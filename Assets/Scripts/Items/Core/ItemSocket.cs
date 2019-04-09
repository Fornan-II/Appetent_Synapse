using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    public EquippedHoldableItem EquippedItem
    {
        get;
        protected set;
    }

    public virtual bool UsePrimary(Pawn user)
    {
        if(EquippedItem)
        {
            return EquippedItem.UsePrimary(user);
        }
        return false;
    }

    public virtual bool UseSecondary(Pawn user)
    {
        if (EquippedItem)
        {
            return EquippedItem.UseSecondary(user);
        }
        return false;
    }

    //Returns the gameobject of the item equipped
    public virtual EquippedHoldableItem Equip(InventoryHoldableItem item)
    {
        if(EquippedItem)
        {
            //Maybe just toggle gameObject on and off, instead of instantiating and destroying
            Destroy(EquippedItem.gameObject);
        }

        if(item)
        {
            if (item.EquippedPrefab)
            {
                GameObject spawnedItem = Instantiate(item.EquippedPrefab, transform);
                EquippedItem = spawnedItem.GetComponent<EquippedHoldableItem>();
            }
        }

        return EquippedItem;
    }
}
