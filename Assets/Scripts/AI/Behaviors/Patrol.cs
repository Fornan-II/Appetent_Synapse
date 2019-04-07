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
    protected bool doWander = true;

    public override void OnEnter(AIController ai)
    {
        movement = ai.GetComponent<AIMoveScript>();
        if (movement)
        {
            movement.pathToDestination = new List<Vector3>(CalculatePath(ai.transform, GetRandomNearbyPoint(ai.transform.position, 15)));
            movement.DoMovement = true;
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

        //Debug line drawing
        Vector3 prevPoint = ai.transform.position;
        Color c = Color.red;
        foreach(Vector3 point in movement.pathToDestination)
        {
            Debug.DrawLine(prevPoint, point, c, ai.treeUpdateInterval * Time.fixedDeltaTime);
            prevPoint = point;
            c = Color.blue;
        }
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
            movement.pathToDestination = new List<Vector3>(CalculatePath(movement.transform, GetRandomNearbyPoint(movement.transform.position, 15)));
            newPathReady = true;
        }
        else
        {
            doWander = false;
            howLongToWait = idleTime;
        }
    }

    //Probably a lot of what is here can get moved to a util class
    protected virtual Vector3 GetRandomNearbyPoint(Vector3 currentPos, float radius)
    {
        Vector2 ranPoint = Random.insideUnitCircle * radius;
        currentPos += new Vector3(ranPoint.x, 0.0f, ranPoint.y);
        NavMeshHit hit;
        if(NavMesh.SamplePosition(currentPos, out hit, 20, -1))
        {
            return hit.position;
        }
        else
        {
            return currentPos;
        }
    }

    protected virtual Vector3[] CalculatePath(Transform t, Vector3 dest)
    {
        Vector3 navPoint;
        if (GetPointOnNavMesh(t.position, out navPoint))
        {
            NavMeshPath path = new NavMeshPath();
            if(NavMesh.CalculatePath(t.position, dest, NavMesh.AllAreas, path))
            {
                return path.corners;
            }
        }
        return new Vector3[0];
    }

    protected virtual bool GetPointOnNavMesh(Vector3 point, out Vector3 result)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 4, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }
}
