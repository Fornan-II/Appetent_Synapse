using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class Sensor : MonoBehaviour
    {
        public AIPawn aiPawn;
        public Pawn.Faction alertWorthyFaction = Pawn.Faction.PLAYER;

        public virtual void Alert(Pawn foundPawn)
        {
            if (foundPawn.MyFaction == alertWorthyFaction && aiPawn)
            {
                aiPawn.GiveAggro(foundPawn, 1);
            }
        }
    }
}