using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resistance Augment", menuName = "Augments/Resistance Augment")]
public class ResistanceAugment : Augment
{
    public float Resistance = 0.4f;

    public override void OnEquip(Inventory equipper)
    {
        DamageReciever dr = equipper.myPawn.GetComponent<DamageReciever>();
        dr.Resistances.GenericResistance.SetModifier("resAugment", Resistance);
        dr.Resistances.ProjectileResistance.SetModifier("resAugment", Resistance);
    }

    public override void OnUnequip(Inventory equipper)
    {
        DamageReciever dr = equipper.myPawn.GetComponent<DamageReciever>();
        dr.Resistances.GenericResistance.RemoveModifier("resAugment");
        dr.Resistances.ProjectileResistance.RemoveModifier("resAugment");
    }
}
