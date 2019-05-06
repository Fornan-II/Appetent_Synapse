using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    public enum DamageZone
    {
        INSIDE,
        OUTSIDE
    }
    public DamageZone WhereToDamage;

    protected virtual void OnTriggerStay(Collider other)
    {
        DamageReciever dr = other.GetComponent<DamageReciever>();

        if(dr && WhereToDamage == DamageZone.INSIDE)
        {
            if(WhereToDamage == DamageZone.INSIDE)
            {
                dr.Die(null);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        DamageReciever dr = other.GetComponent<DamageReciever>();
        if (dr && WhereToDamage == DamageZone.OUTSIDE)
        {
            dr.Die(null);
        }
    }
}
