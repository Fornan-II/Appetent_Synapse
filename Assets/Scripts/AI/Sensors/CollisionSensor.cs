using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CollisionSensor : Sensor
    {
        protected virtual void OnCollisionStay(Collision collision)
        {
            Pawn foundPawn = collision.transform.GetComponent<Pawn>();
            if(foundPawn)
            {
                Alert(foundPawn);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            Pawn foundPawn = other.GetComponent<Pawn>();
            if (foundPawn)
            {
                Alert(foundPawn);
            }
        }
    }
}