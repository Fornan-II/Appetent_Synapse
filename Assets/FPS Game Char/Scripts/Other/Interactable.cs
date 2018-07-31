using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Actor {

    //Only useful for the use of INTERACTLOG() in InteractWith()
    public string interactVerb = "interacts with";

    public bool IgnoresInteraction = false;
    public bool LogInteractEvents = true;

    //This is the method other classes will call when they attempt to interact with this object.
    //This is NOT the class intended to be overriden by inheriting classes.
    public virtual bool InteractWith(Actor source)
    {
        //If this object shouldn't be interacted with currently, no work needs to be done.
        if(IgnoresInteraction)
        {
            return false;
        }

        //Process the unique interaction of this object using ProcessInteraction().
        bool successfulInteract = ProcessInteraction(source);
        if (successfulInteract)
        {
            if (source.Owner)
            {
                INTERACTLOG("Controller " + source.Owner.name + " " + interactVerb + " " + ActorName);
            }
            else
            {
                INTERACTLOG("Actor " + source.name + " " + interactVerb + " " + ActorName);
            }
        }

        return successfulInteract;
    }

    /// <summary>
    /// This is where interactions are processed uniquely for this object.
    /// This is called by InteractWith().
    /// This is the main function to override in inheriting classes.
    /// </summary>
    /// <param name="source"></param>
    /// <returns>Return true if interaction was a success.</returns>
    protected virtual bool ProcessInteraction(Actor source)
    {
        return true;
    }

    public virtual void INTERACTLOG(string s)
    {
        if(LogInteractEvents)
        {
            Debug.Log(s);
        }
    }
}
