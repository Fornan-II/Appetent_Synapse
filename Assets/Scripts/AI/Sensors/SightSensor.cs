using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class SightSensor : Sensor
    {
        public float fieldOfView = 90;
        public float visionDistance = 15.0f;
        public LayerMask layersToCheck = Physics.AllLayers;

        protected virtual void FixedUpdate()
        {
            Collider[] foundCols = Physics.OverlapSphere(transform.position, visionDistance, layersToCheck);

            foreach(Collider c in foundCols)
            {
                SensorTarget target = c.GetComponent<SensorTarget>();
                
                if(target)
                {
                    Vector3 vecToTarget = c.transform.position - transform.position;
                    if(Vector3.Angle(transform.forward, vecToTarget) <= fieldOfView * 0.5f)
                    {
                        int obstacleCount = Physics.RaycastAll(transform.position, vecToTarget, visionDistance, layersToCheck, QueryTriggerInteraction.Ignore).Length;
                        //Debug.Log("Obstacles to possible target " + target.name + ": " + obstacleCount);
                        if (obstacleCount <= 1)
                        {
                            //If we have gotten this far, then the target is within the sight sensor
                            Alert(target.associatedPawn);
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            //https://answers.unity.com/questions/21176/gizmo-question-how-do-i-create-a-field-of-view-usi.html
            float totalFOV = fieldOfView;
            float rayRange = visionDistance;

            float halfFOV = totalFOV / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
            Gizmos.DrawRay(transform.position, transform.forward * rayRange);
            Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
        }
#endif
    }
}