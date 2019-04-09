using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputObject playerInput;
    public uint PlayerNumber
    {
        get
        {
            if(playerInput)
            {
                return playerInput.PlayerNumber;
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField] protected PlayerPawn _controlledPawn;
    public PlayerPawn ControlledPawn
    {
        get { return _controlledPawn; }
        set
        {
            if (value != _controlledPawn)
            {
                OnLoseControl();
                _controlledPawn = value;
                if (value)
                {
                    OnGainControl();
                }
            }
        }
    }
    public bool IsControllingPawn { get { return _controlledPawn; } }

    private void Start()
    {
        if (_controlledPawn)
        {
            OnGainControl();
        }
    }

    protected virtual void Update()
    {
        if (ControlledPawn)
        {
            HandleInput();
        }
    }

    #region Control Change Events
    protected virtual void OnGainControl()
    {
        if (!_controlledPawn) { return; }

        _controlledPawn.PassLockScreen(true);
    }

    protected virtual void OnLoseControl()
    {
        if (!_controlledPawn) { return; }

        ControlledPawn.PassLockScreen(false);
    }
    #endregion

    #region Input
    protected virtual void HandleInput()
    {
        if (!(ControlledPawn && playerInput)) { return; }

        PassMoveInput(playerInput.GetMoveVector());
        PassLookInput(playerInput.GetLookVector());
        PassPrimaryActionInput(playerInput.GetPrimaryAction());
        PassSecondaryActionInput(playerInput.GetSecondaryAction());
        PassDPadInput(playerInput.GetDPadInput());
        PassInteractInput(playerInput.GetInteractInput());
        PassJumpInput(playerInput.GetJumpInput());
        PassSprintInput(playerInput.GetSprintInput());
        PassCrouchInput(playerInput.GetCrouchInput());
        PassStart(playerInput.GetStartInput());
        PassSelect(playerInput.GetBackInput());
    }

    protected virtual void PassMoveInput(Vector2 value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassMoveInput(value);
    }

    protected virtual void PassLookInput(Vector2 value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassLookInput(value);
    }

    protected virtual void PassPrimaryActionInput(float value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassPrimaryActionInput(value);
    }

    protected virtual void PassSecondaryActionInput(float value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassSecondaryActionInput(value);
    }

    protected virtual void PassDPadInput(Vector2 value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassDPadInput(value);
    }

    protected virtual void PassInteractInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassInteractInput(value);
    }

    protected virtual void PassJumpInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassJumpInput(value);
    }

    protected virtual void PassSprintInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassSprintInput(value);
    }

    protected virtual void PassCrouchInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassCrouchInput(value);
    }

    protected virtual void PassStart(bool value)
    {
        //Might not connect to pawn, not sure.
        //Will need to display on the camera of this pawn though, so maybe.
        if (value)
        {
            //Debug.Log(name + " START");
            ControlledPawn.PassLockScreen(!ControlledPawn.MyLookScript.lockState);
        }
    }

    protected virtual void PassSelect(bool value)
    {
        //Might not connect to pawn, not sure.
        //Will need to display on the camera of this pawn though, so maybe.
        if (value)
        {
            Debug.Log(name + " SELECT");
        }
    }
    #endregion
}
