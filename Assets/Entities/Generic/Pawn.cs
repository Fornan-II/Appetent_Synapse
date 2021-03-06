﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public enum Faction
    {
        PLAYER = 2,
        AI = 4
    }
    public Faction MyFaction;

    public Weapon equippedWeapon;

    public Transform defaultBarrel;
    public bool overrideBarrel;

    public virtual void OnKill(DamageReciever victim) { }
}
