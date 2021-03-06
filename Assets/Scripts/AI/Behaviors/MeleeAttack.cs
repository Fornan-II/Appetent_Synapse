﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class MeleeAttack : Behavior
{
    protected Pawn target;
    protected AIMoveScript movement;
    protected MeleeWeapon weapon;
    public Vector3 pathEndPoint;

    protected const float recalculatePathDistance = 5.0f;

    public override void OnEnter(AIController ai)
    {
        target = ai.localBlackboard.GetProperty<Pawn>("target");
        if (target && ai.aiPawn.equippedWeapon is MeleeWeapon && ai.aiPawn.moveScript)
        {
            weapon = ai.aiPawn.equippedWeapon as MeleeWeapon;

            movement = ai.aiPawn.moveScript;

            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            //No target? Can't really attack nothing. Also if no way to move, you can't really get in range to melee
            _currentPhase = StatePhase.EXITING;
        }
    }

    public override void ActiveBehavior(AIController ai)
    {
        //DEBUG LINE DRAWING
        AI.Util.DrawPath(ai.transform.position, movement.pathToDestination, ai.treeUpdateInterval * Time.fixedDeltaTime);

        if (!ai.localBlackboard.GetProperty<bool>(AIPawn.PROPERTY_AGGRO) || target == null)
        {
            _currentPhase = StatePhase.EXITING;
            return;
        }

        bool doPathCalculation = false;
        if((ai.transform.position - target.transform.position).sqrMagnitude > recalculatePathDistance * recalculatePathDistance)
        {
            doPathCalculation = true;
        }

        if((ai.transform.position - target.transform.position).sqrMagnitude <= weapon.reach * weapon.reach)
        {
            weapon.DoAttack(target.gameObject, ai.aiPawn);
        }
        else if(!movement.DoMovement)
        {
            doPathCalculation = true;
        }

        if(doPathCalculation)
        {
            SetPathingToTarget();
        }
    }

    public override void OnExit(AIController ai)
    {
        ai.localBlackboard.SetProperty(AIPawn.PROPERTY_AGGRO, false);
        if(movement)
        {
            movement.DoMovement = false;
        }

        _currentPhase = StatePhase.INACTIVE;
    }

    protected virtual bool SetPathingToTarget()
    {
        if(!movement)
        {
            return false;
        }
        //Debug.Log("Calculating path to target...");
        if (AI.Util.GetPointOnNavMesh(target.transform.position, out pathEndPoint))
        {
            movement.pathToDestination = new List<Vector3>(AI.Util.CalculatePath(movement.transform, pathEndPoint));
            movement.DoMovement = true;
            return true;
        }
        return false;
    }
}
