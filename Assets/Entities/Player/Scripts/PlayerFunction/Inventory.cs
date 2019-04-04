using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryHoldableItem> holdableItems;
    public int selectedHeldItem = 0;
    public ItemSocket heldSocket;

    public List<Augment> augments;

    protected float _previousValue = 0.0f;

    private void Start()
    {
        if(heldSocket && holdableItems.Count > 0)
        {
            heldSocket.Equip(holdableItems[selectedHeldItem]);
        }
    }

    public virtual void ScrollThroughItems(float value)
    {
        if(value != 0.0f && _previousValue == 0.0f && heldSocket && holdableItems.Count > 0)
        {
            if(value < 0.0f)
            {
                selectedHeldItem--;
            }
            if (value > 0.0f)
            {
                selectedHeldItem++;
            }

            //Debug.Log(selectedHeldItem);
            if(selectedHeldItem < 0)
            {
                //Debug.Log("Go to top");
                selectedHeldItem = holdableItems.Count - 1;
            }
            if(selectedHeldItem >= holdableItems.Count)
            {
                //Debug.Log("Go to bottom");
                selectedHeldItem = 0;
            }

            heldSocket.Equip(holdableItems[selectedHeldItem]);
        }

        _previousValue = value;
    }

    bool prevPrimary = false;
    public virtual bool UseEquippedPrimary(bool value, Pawn user)
    {
        bool finalVal = false;

        if(!heldSocket)
        {
            finalVal = false;
        }
        else if (value && !prevPrimary)
        {
            heldSocket.UsePrimary(user);
        }

        prevPrimary = value;
        return finalVal;
    }

    bool prevSecondary = false;
    public virtual bool UseEquippedSecondary(bool value, Pawn user)
    {
        bool finalVal = false;

        if (!heldSocket)
        {
            finalVal = false;
        }
        else if (value && !prevSecondary)
        {
            finalVal = heldSocket.UseSecondary(user);
        }

        prevSecondary = value;
        return finalVal;
    }
}
