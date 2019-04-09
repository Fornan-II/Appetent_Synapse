using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaceratorSword : MeleeWeapon
{
    public float sensitivity = 0.1f;
    
    //Future implementation: target is marked as target. Animation happens, and when OnTriggerEnter() intersects with the collider of target, that is when target takes damage.
    public override bool UsePrimary(Pawn user)
    {
        if(!(user is PlayerPawn))
        {
            return false;
        }
        PlayerPawn playerUser = user as PlayerPawn;

        GameObject target = playerUser.MyInteracter.GetInteractableObject(reach, sensitivity);

        return DoAttack(target, user);
        
    }
}
