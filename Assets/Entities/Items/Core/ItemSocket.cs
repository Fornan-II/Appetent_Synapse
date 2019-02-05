using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    public WorldItem EquippedItem
    {
        get;
        protected set;
    }

   public virtual bool Use(Pawn user)
    {
        return true;
    }

    public virtual void Equip(InventoryItem item)
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
                EquippedItem = spawnedItem.GetComponent<WorldItem>();
            }
        }
    }
}
