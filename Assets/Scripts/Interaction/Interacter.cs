using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    public float Sensitivity;
    public LayerMask InteractionMask;
    public float InteractionRange;
    public Transform interactBarrel;

    #region Redundant extra raycasting methods - for backup only
    public virtual GameObject GetInteractableObject()
    {
        RaycastHit hitInfo;
        if(Physics.SphereCast(interactBarrel.position, Sensitivity, interactBarrel.forward, out hitInfo, InteractionRange, InteractionMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.transform.gameObject;
        }
        return null;
    }

    public virtual GameObject GetInteractableObject(float range)
    {
        RaycastHit hitInfo;
        if(Physics.SphereCast(interactBarrel.position, Sensitivity, interactBarrel.forward, out hitInfo, range, InteractionMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.transform.gameObject;
        }
        return null;
    }

    public virtual GameObject GetInteractableObject(float range, float sensitivity)
    {
        RaycastHit hitInfo;
        if(Physics.SphereCast(interactBarrel.position, sensitivity, interactBarrel.forward, out hitInfo, range, InteractionMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.transform.gameObject;
        }
        return null;
    }
    #endregion

    public virtual void TryToInteract(Pawn source)
    {
        bool doOwnRaycast = true;
        GameObject foundObject = null;
        
        if(source is PlayerPawn)
        {
            PlayerPawn player = source as PlayerPawn;
            if(player.Raycaster)
            {
                if(player.Raycaster.RaycastInfo.DidHit)
                {
                    doOwnRaycast = false;
                    foundObject = player.Raycaster.RaycastInfo.GetHitGameObjectAtRange(InteractionRange);
                }
            }
        }

        if(doOwnRaycast)
        {
            foundObject = GetInteractableObject();
        }
        
        if(foundObject)
        {
            Interactable foundInteractable = foundObject.GetComponent<Interactable>();
            if(foundInteractable)
            {
                if(foundInteractable.InteractWith(source))
                {
                    //So many IFs so that we can play SFX based on outcome.
                    return;
                }
            }
        }
    }
}
