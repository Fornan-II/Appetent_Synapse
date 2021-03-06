﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PlayerPawn myPawn;

    public List<EquippedHoldableItem> holdableItems;
    public int selectedHeldItem = 0;
    public ItemSocket heldSocket;

    public RadialMenu augmentMenu;
    [SerializeField]protected List<Augment> _augments;
    public List<Augment> Augments
    {
        get { return _augments; }
    }
    public Augment SelectedQuickAugment;

    protected float _previousValue = 0.0f;

    protected virtual void Start()
    {
        if(heldSocket && holdableItems.Count > 0)
        {
            heldSocket.Equip(holdableItems[selectedHeldItem], myPawn);
        }

        if(augmentMenu)
        {
            augmentMenu.SetItems(_augments.ToArray());
        }
    }

    protected virtual void Update()
    {
        if(SelectedQuickAugment)
        {
            SelectedQuickAugment.OnUpdate(myPawn);
        }
    }

    #region Held Items
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

            EquipSelectedHeldItem();
        }

        _previousValue = value;
    }

    public virtual void SetWeapon(int index)
    {
        if(0 <= index && index < holdableItems.Count)
        {
            selectedHeldItem = index;
            EquipSelectedHeldItem();
        }
    }

    protected void EquipSelectedHeldItem()
    {
        EquippedHoldableItem newItem = heldSocket.Equip(holdableItems[selectedHeldItem], myPawn);
        if (newItem is Weapon && myPawn)
        {
            myPawn.equippedWeapon = newItem as Weapon;
        }
    }

    public virtual bool UseEquippedPrimary(bool value, Pawn user)
    {
        if(!heldSocket)
        {
            return false;
        }

        heldSocket.UseItem(user, value);
        return true;
    }
    
    public virtual bool UseEquippedSecondary(bool value, Pawn user)
    {
        //USE AUGMENT POWER, IF APPLICABLE
        if (SelectedQuickAugment && user is PlayerPawn && value)
        {
            SelectedQuickAugment.Use(user as PlayerPawn);
            return true;
        }
        return false;
    }
    #endregion

    #region Augments
    public void SetRadialMenuVisible(bool value)
    {
        if (myPawn is PlayerPawn)
        {
            if (value)
            {
                ((PlayerPawn)myPawn).PassLockScreen(false);
            }
            else if (augmentMenu.gameObject.activeSelf)
            {
                ((PlayerPawn)myPawn).PassLockScreen(true);
            }

            augmentMenu.gameObject.SetActive(value);
        }
    }

    public void AddSwappableAugment(Augment aug)
    {
        _augments.Add(aug);
        augmentMenu.AddItem(aug);
    }
    public void RemoveSwappableAugment(Augment aug)
    {
        _augments.Remove(aug);
        augmentMenu.RemoveItem(aug);
    }
    public void SetSwappableAugments(Augment[] augs)
    {
        _augments = new List<Augment>(augs);
        augmentMenu.SetItems(_augments.ToArray());
    }

    public void SetRadialSlot(IRadialSelectable item)
    {
        Augment augmentItem = item as Augment;
        if (augmentItem)
        {
            if (augmentItem != SelectedQuickAugment)
            {
                if (SelectedQuickAugment)
                {
                    SelectedQuickAugment.OnUnequip(this);
                }

                SelectedQuickAugment = item as Augment;
                SelectedQuickAugment.OnEquip(this);
            }
        }
    }
    #endregion
}
