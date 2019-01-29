using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Object", menuName = "InputObject")]
public class InputObject : ScriptableObject
{
    public uint PlayerNumber;

    public string MoveHorizontalAxis = "Horizontal";
    public string MoveVerticalAxis = "Vertical";
    public string LookHorizontalAxis = "Mouse X";
    public string LookVerticalAxis = "Mouse Y";
    public string PrimaryActionAxis;
    public string SecondaryActionAxis;
    public string DPadHorizontalAxis;
    public string DPadVerticalAxis;
    public string InteractButton;
    public string JumpButton = "Jump";
    public string SprintButton;
    public string CrouchButton;
    public string StartButton;
    public string BackButton;

    public Vector2 GetMoveVector()
    {
        Vector2 moveVector = Vector2.zero;
        if (MoveHorizontalAxis != "")
        {
            moveVector.x = Input.GetAxis(MoveHorizontalAxis);
        }
        if (MoveVerticalAxis != "")
        {
            moveVector.y = Input.GetAxis(MoveVerticalAxis);
        }

        return moveVector;
    }

    public Vector2 GetLookVector()
    {
        return new Vector2(Input.GetAxis(LookHorizontalAxis), Input.GetAxis(LookVerticalAxis));
    }

    public float GetPrimaryAction()
    {
        if (PrimaryActionAxis == "")
        {
            return 0.0f;
        }
        return Input.GetAxis(PrimaryActionAxis);
    }

    public float GetSecondaryAction()
    {
        if(SecondaryActionAxis == "")
        {
            return 0.0f;
        }
        return Input.GetAxis(SecondaryActionAxis);
    }

    public Vector2 GetDPadInput()
    {
        Vector2 dPadInput = Vector2.zero;
        if (DPadHorizontalAxis != "")
        {
            dPadInput.x = Input.GetAxis(DPadHorizontalAxis);
        }
        if (DPadVerticalAxis != "")
        {
            dPadInput.y = Input.GetAxis(DPadVerticalAxis);
        }
        return dPadInput;
    }

    public bool GetInteractInput()
    {
        return Input.GetButtonDown(InteractButton);
    }

    public bool GetJumpInput()
    {
        return Input.GetButton(JumpButton);
    }

    public bool GetSprintInput()
    {
        return Input.GetButtonDown(SprintButton);
    }

    public bool GetCrouchInput()
    {
        return Input.GetButtonDown(CrouchButton);
    }

    public bool GetStartInput()
    {
        if(StartButton == "")
        {
            return false;
        }
        return Input.GetButtonDown(StartButton);
    }

    public bool GetBackInput()
    {
        if(BackButton == "")
        {
            return false;
        }
        return Input.GetButtonDown(BackButton);
    }
}
