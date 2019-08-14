using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class MoveScript : MonoBehaviour
{
    #region Pawn Properties
    public Transform movementRelativeTransform;

    //Mobility properties
    public bool allowSprinting = true;
    public bool allowJumping = true;
    public bool allowCrouching = true;
    public float acceleration = 1.0f;
    public bool UseFriction = true; //CURRENTLY DOES NOTHING
    [Range(0, 1)] public float groundedFriction;
    [Range(0, 1)] public float airborneFriction;
    public float sprintMultiplier = 2.0f;
    public float crouchMultiplier = 0.5f;
    public float crouchRate = 0.2f;
    public float jumpForce = 5.0f;
    public float maxGroundAngle = 45;
    public float groundStickCheckDistance = 0.5f;
    public float coyoteTimeDuration = 0.1f;
    public float gravity = 20.0f;
    [HideInInspector]
    public bool letBeGrounded = true;

    public CameraFX fovManager;
    public float sprintFOV = 10.0f;
    public float crouchFOV = 0.0f;

    public EnergyManager energySource;
    public float sprintEnergyCost = 0.0125f;
    public int minimumSprintEnergy = 7;
    #endregion

    #region Pawn Member Variables
    //General components needed to be tracked
    protected Rigidbody _rb;
    protected CapsuleCollider _col;

    //Internal booleans
    protected bool _isCrouching = false;
    protected bool _isSprinting = false;
    protected bool _tryingToJump = false;
    protected bool _isJumping = false;

    //Grounded-related variables
    [SerializeField]protected bool _isGrounded = false;
    protected bool _shouldBeGrounded = false;
    protected Vector3 _groundContactNormal;
    protected float _remainingCoyoteTime;

    //Movement value storage
    protected float _forwardVelocity = 0.0f;
    protected float _strafeVelocity = 0.0f;

    //Crouching-related variables
    protected float _playerHeight;
    protected float _playerInitialScale;
    protected float _crouchPercent = 0.0f;
    #endregion

    protected virtual void Start()
    {
        //Grab initial scale to use in crouching later on
        _playerInitialScale = transform.localScale.y;

        //Get Rigidbody component
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;

        //Grab the main collider of the object (intended to be a CapsuleCollider possibly on a child object) and use it's height as the player's height (used in crouching)
        _col = gameObject.GetComponentInChildren<CapsuleCollider>();
        _playerHeight = _col.height;
    }

    protected virtual void FixedUpdate()
    {
        //_rb.AddForce(new Vector3(0.0f, -gravity * _rb.mass, 0.0f));

        CheckIfGrounded();
        UpdateMoveVelocity();
        HandleCrouching();
    }

    #region Pawn's Controller Inputs

    public virtual void MoveHorizontal(float value)
    {
        _strafeVelocity = value;
    }

    public virtual void MoveVertical(float value)
    {
        _forwardVelocity = value;
    }

    //Make the player jump
    public virtual void Jump(bool value)
    {
        if (value && allowJumping && _isGrounded)
        {
            _tryingToJump = true;
        }
    }

    //Toggles the players' sprint on
    public virtual void Sprint(bool value)
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
    public virtual void Crouch(bool value)
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
                _isCrouching = false;
            }
        }
        else if (value)
        {
            _isCrouching = true;
        }

        if(fovManager)
        {
            if(_isCrouching)
            {
                fovManager.FOV.SetModifier("crouching", crouchFOV);
            }
            else
            {
                fovManager.FOV.RemoveModifier("crouching");
            }
        }
    }
    #endregion

    #region Movement Related Methods
    protected virtual void UpdateMoveVelocity()
    {
        //Initialize moveVelocity to zero. 
        Vector3 desiredVelocity = Vector3.zero;

        //Modify input data to remove issue of faster movement on non-axes
        Vector2 inputVector = Util.VectorInCircleSpace(new Vector2(_forwardVelocity, _strafeVelocity));

        //Apply sprint effects if trying to sprint forwards.
        if(energySource)
        {
            _isSprinting = energySource.Energy >= minimumSprintEnergy && _isSprinting;
        }
        if (_isSprinting && _forwardVelocity > 0.0f)
        {
            inputVector.x *= sprintMultiplier;

            if (fovManager)
            {
                fovManager.FOV.SetModifier("sprinting", sprintFOV);
            }
            if(energySource)
            {
                energySource.DrainRate.SetModifier("sprinting", sprintEnergyCost);
            }
        }
        else
        {
            if (fovManager)
            {
                fovManager.FOV.RemoveModifier("sprinting");
            }
            if(energySource)
            {
                energySource.DrainRate.RemoveModifier("sprinting");
            }
        }

        //Combine the vectors of transform.forward and tranform.right to find the desired move vector.
        //Use modified input data stored in _forwardVelocity and _strafeVelocity as the scalars for these vectors, respectively.
        if (movementRelativeTransform)
        {
            Vector3 correctForward = movementRelativeTransform.forward;
            correctForward.y = 0.0f;
            correctForward.Normalize();
            Vector3 correctRight = movementRelativeTransform.right;
            correctRight.y = 0.0f;
            correctRight.Normalize();

            desiredVelocity = correctForward * inputVector.x + correctRight * inputVector.y;
        }
        else
        {
            desiredVelocity = transform.forward * inputVector.x + transform.right * inputVector.y;
        }
        desiredVelocity.y = 0.0f;

        //Scale velocity by moveSpeed
        desiredVelocity *= acceleration;

        //Scale velocity by crouch multiplier if the player is crouching
        if (_isCrouching)
        {
            desiredVelocity *= crouchMultiplier;
        }

        //Figure out what velocity the player should be moving at.
        Vector3 newVelocity;
        //If the player is grounded and isn't trying to jump, then have them move along the surface they're standing on.
        //Using _shouldBeGrounded instead of _isGrounded because then the groundContactNormal is accurate.
        //Else move the player in the air.
        if (_shouldBeGrounded && !_tryingToJump)
        {
            desiredVelocity.y = _rb.velocity.y;
            desiredVelocity = Vector3.ProjectOnPlane(desiredVelocity, _groundContactNormal);
            newVelocity = Vector3.Lerp(desiredVelocity, _rb.velocity, groundedFriction);
        }
        else
        {
            //If the player is trying to move, then move them in the desired direction. Else, keep them going on their current course.
            //
            if (inputVector.sqrMagnitude > float.Epsilon)
            {
                desiredVelocity.y = _rb.velocity.y;
                newVelocity = Vector3.Lerp(desiredVelocity, _rb.velocity, airborneFriction);
            }
            else
            {
                newVelocity = _rb.velocity;
            }
            //

            //If the player is jumping, then cancel their y velocity and then make them move up at jumpForce. Else, just have gravity effect them.
            //
            if (_tryingToJump)
            {
                newVelocity.y = jumpForce;
                _tryingToJump = false;
                _isGrounded = false;
                _isJumping = true;
            }
            else
            {
                newVelocity.y -= gravity * Time.fixedDeltaTime;
                if (!_isJumping)
                {
                    //Going to need something similar to the ground check, but with a different, slightly further, groundCheckDistance
                    newVelocity = HelpStickToGround(newVelocity);
                }
            }
            //
        }

        //Apply player's new velocity
        _rb.AddForce(newVelocity - _rb.velocity, ForceMode.VelocityChange);
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

    protected virtual Vector3 HelpStickToGround(Vector3 moveVector)
    {
        Vector3 checkPos = _col.transform.position + _col.center;
        float checkDist = (_col.height / 2 - _col.radius) * transform.localScale.y + groundStickCheckDistance;

        Debug.DrawRay(checkPos, Vector3.down * checkDist, Color.red, 3.0f);

        RaycastHit hitInfo;
        if(Physics.SphereCast(checkPos, _col.radius, Vector3.down, out hitInfo, checkDist, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Stairs") || Vector3.Angle(Vector3.up, hitInfo.normal) <= maxGroundAngle)
            {
                moveVector.y = (hitInfo.distance * -1) / Time.fixedDeltaTime;
                Debug.Log("StickSpeed: " + moveVector.y);
            }
        }
        
        return moveVector;
    }
    #endregion

    #region Helper Functions
    private void OnDrawGizmos()
    {
        if(!_col) { return; }

        Vector3 pos1 = _col.transform.position + _col.center;
        Vector3 pos2 = pos1 + (Vector3.down * (_col.height / 2 - _col.radius) * transform.localScale.y);
        float r = _col.radius;
        Gizmos.DrawSphere(pos1, r);
        Gizmos.DrawWireSphere(pos2, r);
    }

    protected virtual void CheckIfGrounded()
    {
        //Prepare data for use in CheckSphere()
        Vector3 checkPos = _col.transform.position + _col.center;
        float checkDist = (_col.height / 2 - _col.radius) * transform.localScale.y * 1.1f;

        //If the player's feet are touching something, player is grounded
        RaycastHit hitInfo;
        _shouldBeGrounded = Physics.SphereCast(checkPos, _col.radius, Vector3.down, out hitInfo, checkDist, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        
        if (!letBeGrounded)
        {
            _shouldBeGrounded = false;
        }
        
        if (_shouldBeGrounded)
        {
            _isJumping = false;

            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.yellow, 1.0f);

            //If ground is too steep (and also not classified as stairs) then the player isn't actually grounded
            _groundContactNormal = hitInfo.normal;
            if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Stairs"))
            {
                if (Vector3.Angle(Vector3.up, _groundContactNormal) > maxGroundAngle)
                {
                    _shouldBeGrounded = false;
                }
            }
        }

        //Check to see if we should start coyote time
        if (_isGrounded && !_shouldBeGrounded && _remainingCoyoteTime <= 0.0f)
        {
            //Start coyote time
            StartCoroutine(CoyoteTimeTimer());
        }
        else if (!_isGrounded)
        {
            _isGrounded = _shouldBeGrounded;
        }
    }

    protected virtual IEnumerator CoyoteTimeTimer()
    {
        _remainingCoyoteTime = coyoteTimeDuration;
        
        while (!_shouldBeGrounded && _remainingCoyoteTime > 0.0f)
        {
            yield return null;
            _remainingCoyoteTime -= Time.deltaTime;
        }
        _remainingCoyoteTime = 0.0f;
        _isGrounded = _shouldBeGrounded;
    }

    protected Coroutine DisableFrictionRoutine;
    public virtual void ReactToKnockback(Pawn kbDealer)
    {
        if(DisableFrictionRoutine != null)
        {
            StopCoroutine(DisableFrictionRoutine);
        }
        DisableFrictionRoutine = StartCoroutine(DisableFrictionTemporarily());
    }

    protected virtual IEnumerator DisableFrictionTemporarily()
    {
        letBeGrounded = false;
        yield return new WaitForSeconds(1.0f);
        letBeGrounded = true;
    }
    #endregion
}