using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class MoveToTarget : Behavior
{
    protected Transform target;
    protected AIMoveScript movement;
    public Vector3 pathEndPoint;

    protected const float recalculatePathDistance = 5.0f;
    public const string PROPERTY_DESIREDTARGETDISTANCE = "DesiredDistanceToTarget";

    public override void OnEnter(AIController ai)
    {
        Pawn targetPawn = ai.localBlackboard.GetProperty<Pawn>("target");
        if(targetPawn && ai.aiPawn.moveScript)
        {
            target = targetPawn.transform;
            movement = ai.aiPawn.moveScript;

            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.EXITING;
        }
    }

    public override void ActiveBehavior(AIController ai)
    {
        bool doPathCalculation = false;

        float sqrDistance = (ai.transform.position - target.transform.position).sqrMagnitude;
        float desiredTargetDistance = ai.localBlackboard.GetProperty<float>(PROPERTY_DESIREDTARGETDISTANCE);

        if (sqrDistance < desiredTargetDistance * desiredTargetDistance)
        {
            _currentPhase = StatePhase.EXITING;
            return;
        }
        else if (sqrDistance > recalculatePathDistance * recalculatePathDistance)
        {
            doPathCalculation = true;
        }

        if (!movement.DoMovement)
        {
            doPathCalculation = true;
        }

        if (doPathCalculation)
        {
            SetPathingToTarget();
        }
    }

    public override void OnExit(AIController ai)
    {
        if(movement)
        {
            movement.DoMovement = false;
        }

        _currentPhase = StatePhase.INACTIVE;
    }

    protected virtual bool SetPathingToTarget()
    {
        if (!movement)
        {
            return false;
        }

        if (AI.Util.GetPointOnNavMesh(target.transform.position, out pathEndPoint))
        {
            movement.pathToDestination = new List<Vector3>(AI.Util.CalculatePath(movement.transform, pathEndPoint));
            movement.DoMovement = true;
            return true;
        }
        return false;
    }
}
