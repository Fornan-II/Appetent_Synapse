using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CollisionSensor : Sensor
    {
        protected virtual void OnCollisionStay(Collision collision)
        {
            SensorTarget foundTarget = collision.transform.GetComponent<SensorTarget>();
            if (foundTarget)
            {
                Alert(foundTarget.associatedPawn);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            SensorTarget foundTarget = other.GetComponent<SensorTarget>();
            if (foundTarget)
            {
                Alert(foundTarget.associatedPawn);
            }
        }
    }
}