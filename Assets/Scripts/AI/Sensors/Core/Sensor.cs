using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class Sensor : MonoBehaviour
    {
        public AIController controller;
        
        public PawnEvent OnSenseEnemyPawn;

        public virtual void Alert(Pawn foundPawn)
        {
            OnSenseEnemyPawn.Invoke(foundPawn);
        }
    }
}