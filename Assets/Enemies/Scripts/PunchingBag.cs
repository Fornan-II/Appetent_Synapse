using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBag : Actor {

    public float accumulatedDamage = 0;

    public override bool TakeDamage(Actor Source, float Value, DamageEventInfo EventInfo = null, Controller Instigator = null)
    {
        accumulatedDamage += Value;
        LOG("Taken " + Value + " damage!\nAccumulated damage: " + accumulatedDamage);
        return base.TakeDamage(Source, Value, EventInfo, Instigator);
    }
}
