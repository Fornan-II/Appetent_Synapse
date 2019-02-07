using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public MoveScript MyMoveScript;
    public LookScript MyLookScript;
    public Camera MyCamera;
    public Inventory MyInventory;
    public Interacter MyInteracter;
    
    [HideInInspector]
    public PlayerController MyController;

    #region Input
    public virtual void PassLockScreen(bool value)
    {
        if (MyLookScript)
        {
            MyLookScript.lockState = value;
        }
    }

    public virtual void PassMoveInput(Vector2 value)
    {
        if(!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed movement input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.MoveHorizontal(value.x);
        MyMoveScript.MoveVertical(value.y);
    }

    public virtual void PassLookInput(Vector2 value)
    {
        if(!MyLookScript)
        {
            Debug.LogWarning(name + " is trying to be passed look input when it has no LookScript component assigned!");
            return;
        }

        MyLookScript.MouseInput = value;
    }

    public virtual void PassPrimaryActionInput(float value)
    {
        if (!MyInventory)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Inventory component assigned!");
            return;
        }

        MyInventory.UseEquippedPrimary(value >= 0.5f, this);
    }

    public virtual void PassSecondaryActionInput(float value)
    {
        if (!MyInventory)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Inventory component assigned!");
            return;
        }

        MyInventory.UseEquippedSecondary(value >= 0.5f, this);
    }

    public virtual void PassDPadInput(Vector2 value)
    {
        if(!MyInventory)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Inventory component assigned!");
            return;
        }
        
        MyInventory.ScrollThroughItems(value.x);
    }
    
    public virtual void PassInteractInput(bool value)
    {
        if(!MyInteracter)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Interacter component assigned!");
            return;
        }

        if(value)
        {
            Debug.Log("interact");
            MyInteracter.TryToInteract(this);
        }
    }

    public virtual void PassJumpInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.Jump(value);
    }

    public virtual void PassSprintInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.Sprint(value);
    }

    public virtual void PassCrouchInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }
        
        MyMoveScript.Crouch(value);
    }
    #endregion
}
