using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRaycastInfo
{
    public Ray Ray;
    public bool DidHit
    {
        get
        {
            return HitInfo.HasValue;
        }
    }
    public RaycastHit? HitInfo;

    public InteractRaycastInfo(Ray ray, bool hit, RaycastHit info)
    {
        Ray = ray;
        if (hit)
        {
            HitInfo = info;
        }
        else
        {
            HitInfo = null;
        }
    }

    public GameObject GetHitGameObject()
    {
        if (DidHit)
        {
            return HitInfo.Value.transform.gameObject;
        }
        return null;
    }

    public GameObject GetHitGameObjectAtRange(float range)
    {
        if (DidHit)
        {
            if (HitInfo.Value.distance <= range)
            {
                return HitInfo.Value.transform.gameObject;
            }
        }
        return null;
    }
}

public class InteractRaycastManager : MonoBehaviour
{
    public float Sensitivity = 0.003f;
    public LayerMask mask = Physics.AllLayers;
    public Transform interactBarrel;
    public float MaxDistance = 100.0f;
    public bool DoRaycast = true;
    
    public InteractRaycastInfo RaycastInfo
    {
        get;
        protected set;
    }

    protected virtual void FixedUpdate()
    {
        if(DoRaycast)
        {
            Ray ray;

            if(interactBarrel)
            {
                ray = new Ray(interactBarrel.transform.position, interactBarrel.transform.forward);
            }
            else
            {
                ray = new Ray(transform.position, transform.forward);
            }

            RaycastHit hitInfo;
            bool didHit = Physics.SphereCast(ray, Sensitivity, out hitInfo, MaxDistance, mask, QueryTriggerInteraction.Ignore);

            RaycastInfo = new InteractRaycastInfo(ray, didHit, hitInfo);
        }
    }
}
