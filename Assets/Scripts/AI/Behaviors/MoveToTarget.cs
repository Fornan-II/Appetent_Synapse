using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using AI.StateMachine;

public static partial class Behaviors
{
    public const string PROPERTY_MoveToTarget_DESIREDTARGETDISTANCE = "DesiredDistanceToTarget";
    private const float MoveToTarget_recalculatePathDistance = 5.0f;

    public static readonly State MoveToTarget = new State()
    {
        Label = "MoveToTarget",
        OnEnter = stateMachine =>
        {
            if (stateMachine.Blackboard.GetProperty<Pawn>("target") && stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn").moveScript)
            {
                stateMachine.AdvancePhase();
            }
            else
            {
                stateMachine.ForceStateInactive();
            }
        },

        Active = stateMachine =>
        {
            Pawn target = stateMachine.Blackboard.GetProperty<Pawn>("target");
            if (target == null)
            {
                stateMachine.ForceEndState();
                return;
            }

            bool doPathCalculation = false;

            AIPawn aiPawn = stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn");
            float sqrDistance = (aiPawn.transform.position - target.transform.position).sqrMagnitude;
            float desiredTargetDistance = stateMachine.Blackboard.GetProperty<float>(PROPERTY_MoveToTarget_DESIREDTARGETDISTANCE);

            if (sqrDistance < desiredTargetDistance * desiredTargetDistance)
            {
                stateMachine.ForceEndState();
                return;
            }
            else if (sqrDistance > MoveToTarget_recalculatePathDistance * MoveToTarget_recalculatePathDistance)
            {
                doPathCalculation = true;
            }

            if (!aiPawn.moveScript.DoMovement)
            {
                doPathCalculation = true;
            }

            if (doPathCalculation)
            {
                AI.Util.SetPathToTarget(aiPawn.moveScript, target.transform.position);
            }
        },

        OnExit = stateMachine =>
        {
            AIPawn aiPawn = stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn");
            if(aiPawn.moveScript)
            {
                aiPawn.moveScript.DoMovement = false;
            }

            stateMachine.AdvancePhase();
        }
    };
}
