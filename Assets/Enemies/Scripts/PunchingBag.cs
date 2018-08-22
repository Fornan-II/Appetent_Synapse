using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBag : Actor {

    public float accumulatedDamage = 0;

    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator, float Knockback)
    {
        accumulatedDamage += Value;
        LOG("Taken " + Value + " damage!\nAccumulated damage: " + accumulatedDamage);
        return base.ProcessDamage(Source, Value, EventInfo, Instigator, Knockback);
    }
}
