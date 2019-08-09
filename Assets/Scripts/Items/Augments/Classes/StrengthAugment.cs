using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strength Augment", menuName = "Augments/Strength Augment")]
public class StrengthAugment : Augment
{
    public float AttackBoost = 1.6f;

    public override void OnEquip(Inventory equipper)
    {
        MeleeWeapon weapon = FindMeleeWeapon(equipper.holdableItems);

        if(weapon)
        {
            weapon.Damage.HitPoints.SetModifier("strengthAugment", AttackBoost);
        }
    }

    public override void OnUnequip(Inventory equipper)
    {
        MeleeWeapon weapon = FindMeleeWeapon(equipper.holdableItems);

        if (weapon)
        {
            weapon.Damage.HitPoints.RemoveModifier("strengthAugment");
        }
    }

    protected virtual MeleeWeapon FindMeleeWeapon(List<EquippedHoldableItem> items)
    {
        MeleeWeapon foundWeapon = null;
        for (int i = 0; i < items.Count && !foundWeapon; i++)
        {
            foundWeapon = items[i].GetComponent<MeleeWeapon>();
        }

        return foundWeapon;
    }
}
