using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public AIMoveScript moveScript;

    protected virtual void FixedUpdate()
    {
        if(moveScript)
        {
            if(moveScript.pathToDestination.Count > 0 && moveScript.DoMovement)
            {
                Vector3 lookAtTarget = moveScript.pathToDestination[0];
                lookAtTarget.y = transform.position.y;

                Vector3 lookDir = lookAtTarget - transform.position;
                transform.forward = Vector3.Slerp(transform.forward, lookDir, 0.1f);
            }
        }
    }
}
