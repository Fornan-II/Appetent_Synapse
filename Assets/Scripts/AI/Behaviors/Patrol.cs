using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using UnityEngine.AI;

public class Patrol : AI.Behavior
{
    public override void OnEnter(AIController ai)
    {
        ai.localBlackboard.SetProperty("PathToNextDestination", CalculatePath(ai.transform, GetRandomNearbyPoint(ai.transform.position, 15)));
        _currentPhase = StatePhase.ACTIVE;
    }

    public override void ActiveBehavior(AIController ai)
    {
        object pathObj = ai.localBlackboard.GetProperty("PathToNextDestination");
        if(pathObj is Vector3[])
        {
            Vector3[] path = pathObj as Vector3[];
            Vector3 prevPoint = ai.transform.position;
            foreach(Vector3 point in path)
            {
                Debug.DrawLine(prevPoint, point, Color.blue, ai.treeUpdateInterval);
                prevPoint = point;
            }
        }
    }

    public override void OnExit(AIController ai)
    {
        _currentPhase = StatePhase.INACTIVE;
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
