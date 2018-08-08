using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Pawn : Game_Pawn
{
    #region Pawn Properties
    public float Health = 100.0f;

    //Mobility properties
    public bool allowSprinting = true;
    public bool allowJumping = true;
    public bool allowCrouching = true;
    public float moveSpeed = 5.0f;
    public float groundedInertia = 0.5f;
    public float aerialInertia = 0.5f;
    public float sprintMultiplier = 2.0f;
    public float crouchMultiplier = 0.5f;
    public float crouchRate = 0.2f;
    public float jumpForce = 5.0f;
    public float maxGroundAngle = 45;

    //Interaction properties
    public float interactRange = 2.0f;
    public float interactSensitivity = 0.1f;

    //FOV modifiers
    public float crouchFOV = 0.8f;
    public float sprintFOV = 1.2f;

    //Head rotation properties
    public float look_xSensitivity = 2.0f;
    public float look_ySensitivity = 2.0f;
    public float look_maxVerticalRotation = -90.0f;
    public float look_minVerticalRotation = 90.0f;

    //Audio sources
    public AudioSource feetAudio;

    //Audio properties
    public AudioClip[] footstepSound;
    public float minFootstepVelocity = 0.01f;
    public float minFootstepBreak = 0.1f;
    public float maxFootstepBreak = 2.0f;
    #endregion

    #region Pawn Member Variables
    //General components needed to be tracked
    protected Rigidbody _rb;
    protected CapsuleCollider _col;

    //Internal booleans
    protected bool _isCrouching = false;
    protected bool _isSprinting = false;
    protected bool _isGrounded = false;
    protected bool _isJumping = false;

    //Set in CheckIfGrounded(), used to determine slope of ground.
    protected Vector3 _groundContactNormal;

    //Movement value storage
    protected float _forwardVelocity = 1.0f;
    protected float _strafeVelocity = 1.0f;
    
    //Crouching-related variables
    protected float _playerHeight;
    protected float _playerInitialScale;
    protected float _crouchPercent = 0.0f;

    //Variables used for modifying camera field of view (FOV)
    protected ModifierTable _fovMultipliers;
    protected int[] _fovKeys; //[0] = modifier given by crouching, [1] = modifier given by sprinting

    //Private so it can't be modified elsewhere. Set to equal Health in Start()
    protected float _maxHealth;

    //Rotation 
    protected float _inputXRotation = 0.0f;
    protected float _inputYRotation = 0.0f;

    //Quaternions used for rotation (head and body rotate seperately)
    protected Quaternion _desiredBodyRotation;
    protected Quaternion _desiredHeadRotation;

    //Audio related
    protected bool _footstepAudioCoroutineIsActive = false;
    #endregion

    protected virtual void Start()
    {
        IsSpectator = false;
        IgnoresDamage = false;
        LogDamageEvents = false;

        //Initialize fov-modifying variables
        _fovMultipliers = new ModifierTable();
        _fovKeys = new int[2];
        for (int i = 0; i < _fovKeys.Length; i++)
        {
            _fovKeys[i] = -1;
        }

        //Grab initial scale to use in crouching later on
        _playerInitialScale = transform.localScale.y;

        //Get Rigidbody component
        _rb = gameObject.GetComponent<Rigidbody>();

        //Starting _desiredBodyRotation is the starting rotation of the object.
        _desiredBodyRotation = transform.rotation;
        //Do the same for the head, if a head exists (which it should)
        if (!head)
        {
            LOG_ERROR("No head object assigned to " + name + "!");
        }
        else
        {
            _desiredHeadRotation = Quaternion.Euler(Vector3.zero);
        }

        //Grab the main collider of the object (intended to be a CapsuleCollider possibly on a child object) and use it's height as the player's height (used in crouching)
        _col = gameObject.GetComponentInChildren<CapsuleCollider>();
        _playerHeight = _col.height;

        _maxHealth = Health;

        //Lock the cursor
        SetCursorLock(_cursorIsLocked);
    }

    protected virtual void FixedUpdate()
    {
        if (CheckIfAlive())
        {
            CheckIfGrounded();
            if(_rb.velocity.sqrMagnitude > minFootstepVelocity && !_footstepAudioCoroutineIsActive && _isGrounded)
            {
                StartCoroutine(HandleFootstepAudio());
            }
            UpdateMoveVelocity();
            HandleCrouching();
            HandleLookRotation();
        }
    }

    #region Pawn's Controller Inputs
    public override void LookHorizontal(float value)
    {
        _inputXRotation = value * look_xSensitivity;
    }

    public override void LookVertical(float value)
    {
        _inputYRotation = value * look_ySensitivity;
    }

    public override void MoveHorizontal(float value)
    {
        _strafeVelocity = value;
    }

    public override void MoveVertical(float value)
    {
        _forwardVelocity = value;
    }

    //Uses the item being held in the dominant hand
    public override void ActionMain(bool value)
    {
        if (value)
        {
            SetCursorLock(true);
            handDominant.UseItem(this);
        }
    }

    //Uses the item being held in the subordinate hand
    public override void ActionSecondary(bool value)
    {
        if (value)
        {
            //Default no action
        }
    }

    //Interacts with the object the player is looking at in the world
    public override void Interact(bool value)
    {
        if (value)
        {
            GameObject highlighted = GetInteractableObject(interactRange, interactSensitivity);
            if (highlighted)
            {
                Interactable other = highlighted.GetComponentInChildren<Interactable>();
                if (other)
                {
                    other.InteractWith(this);
                }
                else
                {
                    Equip(null);
                }
            }
            else
            {
                Equip(null);
            }
        }
    }

    //Make the player jump
    public override void Ability1(bool value)
    {
        if (value && allowJumping && _isGrounded)
        {
            _isJumping = true;
        }
    }

    //Allows the player to sprint when held
    public override void Ability2(bool value)
    {
        if (value && allowSprinting && !_isCrouching)
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
    }

    //Crouches the player when held
    public override void Ability3(bool value)
    {
        if (!allowCrouching) { return; }
        //If currently crouching and the crouch button isn't being held, try to stand up
        //Else crouch.

        if (_isCrouching && !value)
        {
            //Prepare data for use in CheckCapsule()
            Vector3 p1 = _col.transform.position;
            Vector3 p2 = p1 + (Vector3.up * _playerHeight * 0.524f);
            float checkRadius = _col.radius * 0.9f;
            int layermask = 1 << LayerMask.NameToLayer("Player");
            layermask = ~layermask;

            //Check to see if the player has enough room to stand up
            bool didCollide = Physics.CheckCapsule(p1, p2, checkRadius, layermask, QueryTriggerInteraction.Ignore);
            //If there's nothing in their way, let the player stop crouching
            if (!didCollide)
            {
                if (_fovMultipliers.KeyIsActive(_fovKeys[0]))
                {
                    _fovMultipliers.Remove(this, _fovKeys[0]);
                    UpdateCameraManagerFov();
                }
                _isCrouching = false;
            }
        }
        else if (value)
        {
            if (!_fovMultipliers.KeyIsActive(_fovKeys[0]))
            {
                _fovKeys[0] = _fovMultipliers.Add(crouchFOV, this);
                UpdateCameraManagerFov();
            }
            _isCrouching = true;
        }
    }
    #endregion

    #region Movement Related Methods
    protected virtual void UpdateMoveVelocity()
    {
        //Initialize moveVelocity to zero. 
        Vector3 desiredVelocity = Vector3.zero;

        //Modify input data to remove issue of faster movement on non-axes
        Vector2 inputVector = GetProperInputVector();

        //Apply sprint effects if trying to sprint forwards.
        if (_isSprinting && _forwardVelocity > 0.0f)
        {
            inputVector.x *= sprintMultiplier;
            if (!_fovMultipliers.KeyIsActive(_fovKeys[1]))
            {
                _fovKeys[1] = _fovMultipliers.Add(sprintFOV, this);
                UpdateCameraManagerFov();
            }
        }
        else if (_fovMultipliers.KeyIsActive(_fovKeys[1]))
        {
            _fovMultipliers.Remove(this, _fovKeys[1]);
            UpdateCameraManagerFov();
        }

        //Combine the vectors of transform.forward and tranform.right to find the desired move vector.
        //Use modified input data stored in _forwardVelocity and _strafeVelocity as the scalars for these vectors, respectively.
        desiredVelocity = transform.forward * inputVector.x + transform.right * inputVector.y;

        //Scale velocity by moveSpeed
        desiredVelocity *= moveSpeed;

        //Scale velocity by crouch multiplier if the player is crouching
        if (_isCrouching)
        {
            desiredVelocity *= crouchMultiplier;
        }

        if (_isGrounded && !_isJumping)
        {
            desiredVelocity.y = _rb.velocity.y;
            desiredVelocity = Vector3.ProjectOnPlane(desiredVelocity, _groundContactNormal);
            Vector3 newVelocity = Vector3.Lerp(desiredVelocity, _rb.velocity, groundedInertia);
            _rb.velocity = newVelocity;
        }
        else
        {
            if (inputVector.sqrMagnitude > float.Epsilon)
            {
                desiredVelocity.y = _rb.velocity.y;
                _rb.velocity = Vector3.Lerp(desiredVelocity, _rb.velocity, aerialInertia);
            }
            if(_isJumping)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
                _isJumping = false;
            }
        }
        //Debug.DrawRay(transform.position, desiredVelocity, Color.cyan, 1.0f);
        //Debug.DrawRay(transform.position, _rb.velocity, Color.green, 1.0f);
    }

    //Adjusts player height to reflect crouch state
    protected virtual void HandleCrouching()
    {
        float playerHeightScale = Mathf.Lerp(_playerInitialScale, _playerInitialScale * 0.5f, _crouchPercent);
        transform.localScale = new Vector3(1.0f, playerHeightScale, 1.0f);

        if (_isCrouching && _crouchPercent < 1.0f)
        {
            _crouchPercent += Time.fixedDeltaTime * crouchRate;
        }
        else if (!_isCrouching && _crouchPercent > 0.0f)
        {
            _crouchPercent -= Time.fixedDeltaTime * crouchRate;
        }
    }
    #endregion

    #region Mouselook
    protected virtual void HandleLookRotation()
    {
        if (!_cursorIsLocked)
        {
            return;
        }
        _desiredBodyRotation *= Quaternion.Euler(0.0f, _inputYRotation, 0.0f);
        _desiredHeadRotation *= Quaternion.Euler(-_inputXRotation, 0.0f, 0.0f);

        _desiredHeadRotation = ClampRotationAroundX(_desiredHeadRotation);

        transform.localRotation = _desiredBodyRotation;
        head.transform.localRotation = _desiredHeadRotation;
    }

    //Ripped from Unity Standard Assets First Person Character code
    //Thank you for saving me from quaternions
    protected virtual Quaternion ClampRotationAroundX(Quaternion quatIn)
    {
        Quaternion q = quatIn;

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, look_minVerticalRotation, look_maxVerticalRotation);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        //Check for bad values:
        if (float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z))
        {
            return quatIn;
        }

        return q;
    }
    #endregion

    #region Health and Dying
    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        Health -= Value;
        return CheckIfAlive();
    }

    //Returns true if alive, false if dead. Seperate from ProcessDamage so it can be used elsewhere.
    protected virtual bool CheckIfAlive()
    {
        if (Health <= 0)
        {
            Die();
            return false;
        }
        return true;
    }

    //Tell the controller that the pawn has died - default behavior is to unpossess pawn
    protected virtual void Die()
    {
        if (!_controller) { return; }

        Game_Controller GC = (Game_Controller)_controller;
        if (GC)
        {
            GC.PawnHasDied();
        }
        else if (_controller)
        {
            _controller.UnPossesPawn(this);
        }

        //Drop everything equipped and let pawn fall to ground for comic effect
        IgnoresDamage = true;
        if (handDominant) { handDominant.Unequip(); }
        if (handSubordinate) { handSubordinate.Unequip(); }
        _rb.freezeRotation = false;
    }

    public override void OnUnPossession()
    {
        //SetCursorLock(false);
        if (Health > 0)
        {
            _inputXRotation = 0.0f;
            _inputYRotation = 0.0f;
            _strafeVelocity = 0.0f;
            _forwardVelocity = 0.0f;
        }
    }
    #endregion

    #region Helper Functions
    //Useful function simplifying toggling cursor locking
    public override void SetCursorLock(bool newLockState)
    {
        _cursorIsLocked = newLockState;
        if (newLockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    protected virtual void CheckIfGrounded()
    {
        //Prepare data for use in CheckSphere()
        Vector3 checkPos = _col.transform.position;

        //If the player's feet are touching something, player is grounded
        RaycastHit hitInfo;
        _isGrounded = Physics.SphereCast(checkPos, _col.radius, Vector3.down, out hitInfo, _col.height / 2, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (_isGrounded)
        {
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.yellow, 1.0f);
            _groundContactNormal = hitInfo.normal;
            if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Stairs"))
            {
                if(Vector3.Angle(Vector3.up, _groundContactNormal) > maxGroundAngle)
                {
                    _isGrounded = false;
                }
            }
        }
    }

    protected virtual Vector2 GetProperInputVector()
    {
        Vector2 inputVector = new Vector2(_forwardVelocity, _strafeVelocity);
        Vector2 maxedVector = Vector2.one;

        //Find maximum value 
        if (Mathf.Abs(_forwardVelocity) > Mathf.Abs(_strafeVelocity))
        {
            maxedVector.Set(1.0f, _strafeVelocity / _forwardVelocity);
            if (_forwardVelocity < 0.0f)
            {
                maxedVector.x = -1.0f;
            }
        }
        else if (Mathf.Abs(_forwardVelocity) < Mathf.Abs(_strafeVelocity))
        {
            maxedVector.Set(_forwardVelocity / _strafeVelocity, 1.0f);
            if (_strafeVelocity < 0.0f)
            {
                maxedVector.y = -1.0f;
            }
        }

        inputVector /= maxedVector.magnitude;

        return new Vector3(inputVector.x, inputVector.y);
    }

    protected virtual void UpdateCameraManagerFov()
    {
        CameraManager cm = head.GetComponent<CameraManager>();
        if(!cm)
        {
            LOG_ERROR("No CameraManager on head!");
            return;
        }
        cm.ScaleFovBy(_fovMultipliers.Product());
    }
    #endregion

    #region Audio
    //Make the sound play frequently, with a faster velocity meaning a higher frequency. Also the player must be grounded.
    protected virtual IEnumerator HandleFootstepAudio()
    {
        _footstepAudioCoroutineIsActive = true;
        float activeTimer = minFootstepBreak + 1;
        float maximumSquareVelocity = moveSpeed * moveSpeed * sprintMultiplier * sprintMultiplier;
        float timeUntilNextSound;

        do
        {
            timeUntilNextSound = Mathf.Lerp(maxFootstepBreak, minFootstepBreak, _rb.velocity.sqrMagnitude / maximumSquareVelocity);
            if (activeTimer >= timeUntilNextSound)
            {
                if (footstepSound.Length != 0)
                {
                    feetAudio.clip = SelectClipFrom(footstepSound);
                }
                feetAudio.Play();
                activeTimer = 0.0f;
            }
            yield return null;
            activeTimer += Time.deltaTime;
        } while (_rb.velocity.sqrMagnitude > minFootstepVelocity && _isGrounded) ;
        _footstepAudioCoroutineIsActive = false;
    }

    AudioClip SelectClipFrom(AudioClip[] arr)
    {
        if(arr.Length == 1)
        {
            return arr[0];
        }

        int index = (int)Random.Range(0, arr.Length - 1);
        return arr[index];
    }
    #endregion
}