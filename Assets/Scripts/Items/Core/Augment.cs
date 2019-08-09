using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Augment : ScriptableObject, IRadialSelectable
{
    [SerializeField] protected Sprite _radialIcon;
    [SerializeField] protected bool _canUseFixedSlot;
    public bool CanUseFixedSlot { get { return _canUseFixedSlot; } }
    
    public Sprite GetIcon()
    {
        return _radialIcon;
    }

    public virtual void Use(PlayerPawn user)
    {

    }

    public virtual void OnUpdate(PlayerPawn user)
    {

    }

    public virtual void OnEquip(Inventory equipper)
    {
        
    }

    public virtual void OnUnequip(Inventory equipper)
    {

    }
}