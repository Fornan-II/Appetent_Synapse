using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : UnityEvent<Pawn> { }

public class Interactable : MonoBehaviour
{
    public bool IgnoresInteraction = false;

    public InteractEvent OnInteract;

    public virtual bool InteractWith(Pawn source)
    {
        if(IgnoresInteraction)
        {
            return false;
        }

        OnInteract.Invoke(source);
        return true;
    }
}
