using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaceratorSword : Item {

    public float damage = 10.0f;
    public float reach = 2.0f;
    public float sensitivity = 0.1f;
    
    public override bool Use(Actor user)
    {
        Game_Pawn GP = (Game_Pawn)user;
        GameObject target = GP.GetInteractableObject(reach, sensitivity);
        base.Use(user);
        if(target)
        {
            Actor targetActor = target.GetComponent<Actor>();
            if(targetActor)
            {
                targetActor.TakeDamage(user, damage);
                return true;
            }
        }
        //If no actor found to take damage, return false.
        return false;
    }
}
