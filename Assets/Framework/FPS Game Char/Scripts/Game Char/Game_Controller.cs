using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : PlayerController {

    public bool allowPausing = true;
    public bool allowControlOverPawn = true;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        LogInputStateInfo = false;
        LogHUDUpdateError = false;
    }

    #region Controls Related
    public override void DefaultBinds()
    {
        AddAxis("LookHorizontal",       LookHorizontal);
        AddAxis("LookVertical",         LookVertical);
        AddAxis("MoveHorizontal",       Horizontal);
        AddAxis("MoveVertical",         Vertical);
        AddButton("ActionMain",         Fire1);
        AddButton("ActionSecondary",    Fire2);
        AddButton("Interact",           Fire3);
        AddButton("Ability1",           Fire4);
        AddButton("Ability2",           Fire5);
        AddButton("Ability3",           Fire6);
        AddButton("Cancel",             Cancel);
    }

    public virtual void LookHorizontal(float value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if(GP && allowControlOverPawn)
        {
            GP.LookHorizontal(value);
        }
    }

    public virtual void LookVertical(float value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.LookVertical(value);
        }
    }

    //MoveHorizontal
    public override void Horizontal(float value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.MoveHorizontal(value);
        }
    }

    //MoveVertical
    public override void Vertical(float value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.MoveVertical(value);
        }
    }

    //ActionMain
    public override void Fire1(bool value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.ActionMain(value);
        }
    }

    //ActionSecondary
    public override void Fire2(bool value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.ActionSecondary(value);
        }
    }

    //Interact
    public override void Fire3(bool value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.Interact(value);
        }
    }

    //Ability1
    public override void Fire4(bool value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.Ability1(value);
        }
    }

    //Ability2
    public virtual void Fire5(bool value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if (GP && allowControlOverPawn)
        {
            GP.Ability2(value);
        }
    }

    //Ability3
    public virtual void Fire6(bool value)
    {
        Game_Pawn GP = (Game_Pawn)PossesedPawn;
        if(GP && allowControlOverPawn)
        {
            GP.Ability3(value);
        }
    }
    public virtual void Cancel(bool value)
    {
        if(value)
        {
            Game_Pawn GP = (Game_Pawn)PossesedPawn;
            //Used to pause the game and check to see if menu system existed because it would GP.SetCursorLock(!ms.IsPaused).
            //Now, only disables cursor lock.
            if (GP && allowPausing)
            {
                GP.SetCursorLock(false);
                //LOG("Escape");
            }
        }
    }
    #endregion

    //Called from pawn's Die(). Meant to be overriden.
    public virtual void PawnHasDied()
    {
        UnPossesPawn(PossesedPawn);
    }
}
