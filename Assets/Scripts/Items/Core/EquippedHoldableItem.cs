using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedHoldableItem : MonoBehaviour
{
    public virtual bool UsePrimary(Pawn source)
    {
        return true;
    }

    public virtual bool UseSecondary(Pawn source, bool value = true)
    {
        return true;
    }
}
