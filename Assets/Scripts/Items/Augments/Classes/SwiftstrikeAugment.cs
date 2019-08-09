using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Swiftstrike Augment", menuName = "Augments/Swiftstrike Augment")]
public class SwiftstrikeAugment : Augment
{
    public override void OnEquip(Inventory equipper)
    {
        MeleeWeapon weapon = FindMeleeWeapon(equipper.holdableItems);

        if(weapon)
        {
            weapon.AttackSpeed.SetModifier("swiftstrikeAugment", 0.0f);
        }
    }

    public override void OnUnequip(Inventory equipper)
    {
        MeleeWeapon weapon = FindMeleeWeapon(equipper.holdableItems);

        if (weapon)
        {
            weapon.AttackSpeed.RemoveModifier("swiftstrikeAugment");
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
