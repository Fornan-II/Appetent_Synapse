using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedHoldableItem : MonoBehaviour
{
    protected enum UseMode
    {
        HOLD,
        TAP
    }
    [SerializeField] protected UseMode Mode;
    protected bool _previousUseValue = false;

    public void UseItem(Pawn source, bool value)
    {
        //Call use if the use button press state lines up with this item's UseMode
        if(value && (Mode == UseMode.HOLD || (Mode == UseMode.TAP && !_previousUseValue)))
        {
            Use(source);
        }
        else if(_previousUseValue)
        {
            OnUseDone(source);
        }

        _previousUseValue = value;
    }

    protected virtual void Use(Pawn source)
    {

    }

    protected virtual void OnUseDone(Pawn source)
    {

    }

    public virtual void OnEquip(Pawn source) { }

    public virtual void OnUnequip(Pawn source)
    {
        _previousUseValue = false;
    }
}
