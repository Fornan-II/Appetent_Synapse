using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Steadfast Augment", menuName = "Augments/Steadfast Augment")]
public class SteadfastAugment : Augment
{
    public override void OnEquip(Inventory equipper)
    {
        DamageReciever dr = equipper.myPawn.GetComponent<DamageReciever>();
        dr.Resistances.KnockbackResistance.SetModifier("steadfastAugment", 1.0f - dr.Resistances.KnockbackResistance.Value);
    }

    public override void OnUnequip(Inventory equipper)
    {
        DamageReciever dr = equipper.myPawn.GetComponent<DamageReciever>();
        dr.Resistances.GenericResistance.RemoveModifier("steadfastAugment");
    }
}
