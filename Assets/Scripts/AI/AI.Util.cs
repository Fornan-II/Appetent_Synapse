using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace AI
{
    public static class Util
    {
        public static Vector3 GetRandomNearbyPoint(Vector3 currentPos, float radius)
        {
            Vector2 ranPoint = Random.insideUnitCircle * radius;
            currentPos += new Vector3(ranPoint.x, 0.0f, ranPoint.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(currentPos, out hit, radius, -1))
            {
                return hit.position;
            }
            else
            {
                return currentPos;
            }
        }

        public static Vector3[] CalculatePath(Transform t, Vector3 dest)
        {
            Vector3 navPoint;
            if (GetPointOnNavMesh(t.position, out navPoint))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(t.position, dest, NavMesh.AllAreas, path))
                {
                    return path.corners;
                }
            }
            return new Vector3[0];
        }

        public static bool GetPointOnNavMesh(Vector3 point, out Vector3 result, float sampleDistance = 4.0f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(point, out hit, sampleDistance, NavMesh.AllAreas))
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

        public static void DrawPath(Vector3 currentPos, List<Vector3> pathPoints, float stayTime)
        {
            //Debug line drawing
            Vector3 prevPoint = currentPos;
            Color c = Color.red;
            foreach (Vector3 point in pathPoints)
            {
                Debug.DrawLine(prevPoint, point, c, stayTime);
                prevPoint = point;
                c = Color.blue;
            }
        }
    }
}
