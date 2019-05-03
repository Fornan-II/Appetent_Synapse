﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class Sensor : MonoBehaviour
    {
        public AIController controller;
        public Pawn.Faction alertWorthyFaction = Pawn.Faction.PLAYER;

        public PawnEvent OnSenseEnemyPawn;

        public virtual void Alert(Pawn foundPawn)
        {
            if (foundPawn.MyFaction == alertWorthyFaction)
            {
                OnSenseEnemyPawn.Invoke(foundPawn);
            }
        }
    }
}