using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quiver Augment", menuName = "Augments/Quiver Augment")]
public class QuiverAugment : Augment
{
    public int MaximumAmmo = 10;
    public float TimeBetweenAmmo = 3.0f;

    protected float _timeSinceLastGivenAmmo = 0.0f;

    public override void OnEquip(Inventory equipper)
    {
        _timeSinceLastGivenAmmo = 0.0f;
    }

    public override void OnUpdate(PlayerPawn user)
    {
        if(!user.MyInventory) { return; }
        
        if(_timeSinceLastGivenAmmo >= TimeBetweenAmmo)
        {
            GiveAmmo(user.MyInventory);
            _timeSinceLastGivenAmmo = 0.0f;
        }

        _timeSinceLastGivenAmmo += Time.deltaTime;
    }

    protected void GiveAmmo(Inventory playerInventory)
    {
        RangedWeapon foundWeapon = null;
        for(int i = 0; i < playerInventory.holdableItems.Count && !foundWeapon; i++)
        {
            foundWeapon = playerInventory.holdableItems[i].GetComponent<RangedWeapon>();
        }

        if(foundWeapon)
        {
            if (foundWeapon.ammo < MaximumAmmo)
            {
                foundWeapon.ammo++;
            }
        }
    }
}
