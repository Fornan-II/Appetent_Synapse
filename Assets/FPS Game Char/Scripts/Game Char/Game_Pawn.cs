using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Pawn : Pawn {

    public bool logInputValues = false;

    protected bool _cursorIsLocked = true;

    private void Start()
    {
        logInputValues = true;
    }

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
}
