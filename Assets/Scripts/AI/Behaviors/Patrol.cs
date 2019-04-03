using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using UnityEngine.AI;

public class Patrol : AI.Behavior
{
    public override void OnEnter(Blackboard b)
    {
        object T = b.GetProperty("Transform");
        if (T is Transform)
        {
            Transform transform = T as Transform;
            b.SetProperty("PathToNextDestination", CalculatePath(transform, GetRandomNearbyPoint(transform.position, 15)));
            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.INACTIVE;
        }
    }

    public override void ActiveBehavior(Blackboard b)
    {
        object pathObj = b.GetProperty("PathToNextDestination");
        object T = b.GetProperty("Transform");
        if(pathObj is Vector3[] && T is Transform)
        {
            Vector3[] path = pathObj as Vector3[];
            Vector3 prevPoint = (T as Transform).position;
            foreach(Vector3 point in path)
            {
                Debug.DrawLine(prevPoint, point, Color.blue);
                prevPoint = point;
            }
        }
    }

    public override void OnExit(Blackboard b)
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
