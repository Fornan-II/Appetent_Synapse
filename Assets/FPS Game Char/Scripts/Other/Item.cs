﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

    public string useVerb = "uses";
    public Game_Pawn _wielder = null;

    //Reset is called when the user hits the Reset button in the Inspector's context menu or when adding the component the first time. Part of MonoBehaviour.
    protected virtual void Reset()
    {
        interactVerb = "picks up";
    }

    /// <summary>
    /// The interaction this item will have if it is in the world and not equipped (usually).
    /// The basic interaction of an item while it's in the world is to have the FPS_Pawn "source" equip it.
    /// </summary>
    /// <param name="source"></param>
    /// <returns>Return true if the item is equipped</returns>
    protected override bool ProcessInteraction(Actor source)
    {
        Game_Pawn GP = source.GetComponent<Game_Pawn>();
        if(GP)
        {
            if(GP.Equip(this))
            {
                _wielder = GP;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// The method meant to be called when player has equipped the item.
    /// This is the main function to override in inheriting classes.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual bool Use(Actor user)
    {
        INTERACTLOG(user.name + " " + useVerb + " " + ActorName);
        return true;
    }

    public virtual void SetNotBeingHeld()
    {
        _wielder = null;
    }
}
