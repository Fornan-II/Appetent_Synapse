using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public virtual bool UsePrimary(Pawn source)
    {
        return true;
    }

    public virtual bool UseSecondary(Pawn source)
    {
        return true;
    }
}
