using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFullChargeRequired : MeleeWeapon
{
    public override bool DoAttack(GameObject target, Pawn user)
    {
        if(AttackCharge >= 1.0f)
        {
            return base.DoAttack(target, user);
        }

        return false;
    }
}
