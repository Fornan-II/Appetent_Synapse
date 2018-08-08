using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Pawn : Pawn {

    #region Pawn Properties
    public bool logInputValues = false;

    //Important character gameObjects
    public GameObject head;
    public Socket handDominant;
    public Socket handSubordinate;
    #endregion

    #region Pawn Member Variables
    protected bool _cursorIsLocked = true;

    #endregion

    private void Start()
    {
        logInputValues = true;
    }

    #region Input Methods
    public virtual void LookHorizontal(float value)
    {
        if(logInputValues && value != 0.0f)
        {
            LOG(name + " : Game_Pawn : LookHorizontal : " + value);
        }
    }

    public virtual void LookVertical(float value)
    {
        if (logInputValues && value != 0.0f)
        {
            LOG(name + " : Game_Pawn : LookVertical : " + value);
        }
    }

    public virtual void MoveHorizontal(float value)
    {
        if (logInputValues && value != 0.0f)
        {
            LOG(name + " : Game_Pawn : MoveHorizontal : " + value);
        }
    }

    public virtual void MoveVertical(float value)
    {
        if (logInputValues && value != 0.0f)
        {
            LOG(name + " : Game_Pawn : MoveVertical : " + value);
        }
    }

    public virtual void ActionMain(bool value)
    {
        if (logInputValues && value)
        {
            LOG(name + " : Game_Pawn : ActionMain : " + value);
        }
    }

    public virtual void ActionSecondary(bool value)
    {
        if (logInputValues && value)
        {
            LOG(name + " : Game_Pawn : ActionSecondary : " + value);
        }
    }

    public virtual void Interact(bool value)
    {
        if (logInputValues && value)
        {
            LOG(name + " : Game_Pawn : Interact : " + value);
        }
    }

    public virtual void Ability1(bool value)
    {
        if (logInputValues && value)
        {
            LOG(name + " : Game_Pawn : Ability1 : " + value);
        }
    }

    public virtual void Ability2(bool value)
    {
        if (logInputValues && value)
        {
            LOG(name + " : Game_Pawn : Ability2 : " + value);
        }
    }

    public virtual void Ability3(bool value)
    {
        if (logInputValues && value)
        {
            LOG(name + " : Game_Pawn : Ability3 : " + value);
        }
    }
#endregion

    #region Helper Functions
    public virtual bool Equip(Item item)
    {
        //It's really hard to put something into a hand you don't have
        if (!handDominant)
        {
            return false;
        }

        //If player is already holding something, drop it.
        if (handDominant.HasItem)
        {
            handDominant.Unequip();
        }
        //If there's no item to equip, there's nothing else to be done.
        if (!item)
        {
            return true;
        }
        //Equip the item
        return handDominant.Equip(item);
    }

    //Used to get the object the player is looking at
    public virtual GameObject GetInteractableObject(float range, float sensitivity)
    {
        //Spherecast to determine what the player is looking at
        //Uses spherecast instead of raycast to make it make it easier and require less precision aiming.
        int layermask = 1 << LayerMask.NameToLayer("Player");
        layermask = ~layermask;

        RaycastHit hitInfo;
        //Raycast version
        //Physics.Raycast(head.transform.position, head.transform.forward, out hitInfo, range, layermask, QueryTriggerInteraction.Ignore);
        //Spherecast version
        Physics.SphereCast(head.transform.position, sensitivity, head.transform.forward, out hitInfo, range, layermask, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider)
        {
            return hitInfo.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    //Useful function simplifying toggling cursor locking
    public virtual void SetCursorLock(bool newLockState)
    {
        _cursorIsLocked = newLockState;
        if(!newLockState)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    #endregion
}
