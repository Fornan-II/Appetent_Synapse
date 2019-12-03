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

    public virtual bool UsePrimary(Pawn user, bool value)
    {
        if(EquippedItem)
        {
            return EquippedItem.UsePrimary(user, value);
        }
        return false;
    }

    public virtual bool UseSecondary(Pawn user, bool value)
    {
        if (EquippedItem)
        {
            return EquippedItem.UseSecondary(user, value);
        }
        return false;
    }

    //Returns the gameobject of the item equipped
    public virtual EquippedHoldableItem Equip(EquippedHoldableItem item, Pawn equipper)
    {
        if (EquippedItem)
        {
            //Maybe just toggle gameObject on and off, instead of instantiating and destroying
            EquippedItem.OnUnequip(equipper);
            EquippedItem.gameObject.SetActive(false);
            EquippedItem = null;
        }

        if (item)
        {
            item.gameObject.SetActive(true);
            EquippedItem = item;
            EquippedItem.OnEquip(equipper);
        }

        return EquippedItem;
    }

    #region Deprecated
    //Deprecated version that used InventoryHoldableItem
    /*public virtual EquippedHoldableItem Equip(InventoryHoldableItem item, Pawn equipper)
    {
        if(EquippedItem)
        {
            //Maybe just toggle gameObject on and off, instead of instantiating and destroying
            EquippedItem.OnUnequip(equipper);
            Destroy(EquippedItem.gameObject);
        }

        if(item)
        {
            if (item.EquippedPrefab)
            {
                GameObject spawnedItem = Instantiate(item.EquippedPrefab, transform);
                EquippedItem = spawnedItem.GetComponent<EquippedHoldableItem>();
                if(EquippedItem)
                {
                    EquippedItem.OnEquip(equipper);
                }
            }
        }

        return EquippedItem;
    }*/
    #endregion
}
