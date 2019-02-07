using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Augment : MonoBehaviour
{
    public abstract void ApplyEffect(Pawn user);
}