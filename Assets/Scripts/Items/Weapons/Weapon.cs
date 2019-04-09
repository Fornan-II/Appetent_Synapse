using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : EquippedHoldableItem
{
    public abstract bool DoAttack(GameObject target, Pawn user);
}
