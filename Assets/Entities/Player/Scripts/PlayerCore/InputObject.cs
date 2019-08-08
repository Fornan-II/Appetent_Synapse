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
    public string Scroll;
    public string WeaponHotkey1;
    public string WeaponHotkey2;
    public string WeaponHotkey3;
    public string InteractButton;
    public string InventoryButton;
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

    public float GetScrollInput()
    {
        if(Scroll == "")
        {
            return 0.0f;
        }
        return Input.GetAxis(Scroll);
    }

    public int GetWeaponHotkey()
    {
        int weaponKey = -1;
        if(WeaponHotkey3 != "")
        {
            if (Input.GetButtonDown(WeaponHotkey3))
            {
                weaponKey = 2;
            }
        }
        if (WeaponHotkey2 != "")
        {
            if (Input.GetButtonDown(WeaponHotkey2))
            {
                weaponKey = 1;
            }
        }
        if (WeaponHotkey1 != "")
        {
            if (Input.GetButtonDown(WeaponHotkey1))
            {
                weaponKey = 0;
            }
        }

        return weaponKey;
    }

    public bool GetInteractInput()
    {
        return Input.GetButtonDown(InteractButton);
    }

    public bool GetInventoryInput()
    {
        if(InventoryButton == "")
        {
            return false;
        }
        return Input.GetButton(InventoryButton);
    }

    public bool GetJumpInput()
    {
        return Input.GetButton(JumpButton);
    }

    public bool GetSprintInput()
    {
        return Input.GetButton(SprintButton);
    }

    public bool GetCrouchInput()
    {
        return Input.GetButton(CrouchButton);
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
