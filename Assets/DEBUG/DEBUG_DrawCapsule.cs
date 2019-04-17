using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_DrawCapsule : MonoBehaviour
{
    public CapsuleCollider capsule;

    private void OnValidate()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    private void OnDrawGizmosSelected()
    {
        if(!capsule)
        {
            return;
        }

        float centerToPointsDistance = (capsule.height * 0.5f) - capsule.radius;
        Vector3 point1 = transform.position + transform.right * capsule.center.x + transform.up * capsule.center.y + transform.forward * capsule.center.z;
        Vector3 point2 = transform.position + transform.right * capsule.center.x + transform.up * capsule.center.y + transform.forward * capsule.center.z;
        //X = 0, Y = 1, Z = 2
        switch (capsule.direction)
        {
            case 0:
                {
                    Vector3 offset = transform.right * centerToPointsDistance;
                    point1 += offset;
                    point2 -= offset;
                    break;
                }
            case 1:
                {
                    Vector3 offset = transform.up * centerToPointsDistance;
                    point1 += offset;
                    point2 -= offset;
                    break;
                }
            case 2:
                {
                    Vector3 offset = transform.forward * centerToPointsDistance;
                    point1 += offset;
                    point2 -= offset;
                    break;
                }
        }

        Gizmos.DrawWireSphere(point1, capsule.radius);
        Gizmos.DrawWireSphere(point2, capsule.radius);
    }
}
