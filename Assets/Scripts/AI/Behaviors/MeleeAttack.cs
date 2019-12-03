using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using AI.StateMachine;

public static partial class Behaviors
{
    private const float MeleeAttack_recalculatePathDistance = 5.0f;

    public static readonly State MeleeAttack = new State()
    {
        Label = "MeleeAttack",
        OnEnter = stateMachine =>
        {
            Pawn target = stateMachine.Blackboard.GetProperty<Pawn>("target");
            AIPawn aiPawn = stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn");
            if (target && aiPawn.equippedWeapon is MeleeWeapon && aiPawn.moveScript)
            {
                stateMachine.AdvancePhase();
            }
            else
            {
                //No target? Can't really attack nothing. Also if no way to move, you can't really get in range to melee
                stateMachine.ForceStateInactive();
            }
        },

        Active = stateMachine =>
        {
            Pawn target = stateMachine.Blackboard.GetProperty<Pawn>("target");
            AIPawn aiPawn = stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn");

            if (!(stateMachine.Blackboard.GetProperty<bool>(AIPawn.PROPERTY_AGGRO) && target && aiPawn.equippedWeapon))
            {
                stateMachine.AdvancePhase();
                return;
            }

            bool doPathCalculation = false;
            float sqrDistToTarget = (aiPawn.transform.position - target.transform.position).sqrMagnitude;
            if (sqrDistToTarget > MeleeAttack_recalculatePathDistance * MeleeAttack_recalculatePathDistance)
            {
                doPathCalculation = true;
            }

            MeleeWeapon meleeWeapon = aiPawn.equippedWeapon as MeleeWeapon;
            if (sqrDistToTarget <= meleeWeapon.reach * meleeWeapon.reach)
            {
                meleeWeapon.DoAttack(target.gameObject, aiPawn);
            }
            else if (!aiPawn.moveScript.DoMovement)
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
            if (aiPawn.moveScript)
            {
                aiPawn.moveScript.DoMovement = false;
            }
        }
    };
}
