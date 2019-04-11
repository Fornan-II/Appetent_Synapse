using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using UnityEngine.AI;

public class Patrol : AI.Behavior
{
    protected AIMoveScript movement;
    protected bool newPathReady = false;

    protected float howLongToWait;
    protected bool doWander = false;

    public override void OnEnter(AIController ai)
    {
        if (ai.aiPawn.moveScript)
        {
            movement = ai.aiPawn.moveScript;
            //movement.pathToDestination = new List<Vector3>(AI.Util.CalculatePath(ai.transform, AI.Util.GetRandomNearbyPoint(ai.transform.position, 15)));
            //movement.DoMovement = true;
            howLongToWait = Random.Range(5.0f, 10.0f);
            movement.OnPathComplete += GoGetPath;
            movement.OnGiveUpPathing += GoGetPath;
            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.INACTIVE;
        }
    }

    public override void ActiveBehavior(AIController ai)
    {
        //Decide between idling and wandering around
        if(!doWander)
        {
            if (howLongToWait <= 0.0f)
            {
                GoGetPath();
            }
            howLongToWait -= Time.fixedDeltaTime * ai.treeUpdateInterval;
        }
        else if(newPathReady)
        {
            movement.DoMovement = true;
            newPathReady = false;
        }

        //DEBUG LINE DRAWING
        AI.Util.DrawPath(ai.transform.position, movement.pathToDestination, ai.treeUpdateInterval * Time.fixedDeltaTime);
    }

    public override void OnExit(AIController ai)
    {
        movement.DoMovement = false;
        _currentPhase = StatePhase.INACTIVE;
    }

    protected virtual void GoGetPath()
    {
        float idleTime = Random.Range(0.0f, 10.0f);
        if (idleTime < 5.0f)
        {
            doWander = true;
            movement.pathToDestination = new List<Vector3>(AI.Util.CalculatePath(movement.transform, AI.Util.GetRandomNearbyPoint(movement.transform.position, 15)));
            newPathReady = true;
        }
        else
        {
            doWander = false;
            howLongToWait = idleTime;
        }
    }
}
