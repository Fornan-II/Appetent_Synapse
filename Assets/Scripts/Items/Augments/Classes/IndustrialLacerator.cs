using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Industrial Lacerator Augment", menuName = "Augments/Industrial Lacerator Augment")]
public class IndustrialLacerator : Augment
{
    public override void OnEquip(Inventory equipper)
    {
        LaceratorSword lacerator = FindLacerator(equipper.holdableItems);

        if(lacerator)
        {
            lacerator.Damage.Type = DamagePacket.DamageType.STRUCTURAL;
        }
    }

    public override void OnUnequip(Inventory equipper)
    {
        LaceratorSword lacerator = FindLacerator(equipper.holdableItems);

        if (lacerator)
        {
            lacerator.Damage.Type = DamagePacket.DamageType.GENERIC;
        }
    }

    protected virtual LaceratorSword FindLacerator(List<EquippedHoldableItem> items)
    {
        LaceratorSword foundWeapon = null;
        for (int i = 0; i < items.Count && !foundWeapon; i++)
        {
            foundWeapon = items[i].GetComponent<LaceratorSword>();
        }

        return foundWeapon;
    }
}
