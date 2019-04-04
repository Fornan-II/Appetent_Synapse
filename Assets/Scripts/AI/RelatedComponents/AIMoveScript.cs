using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIMoveScript : MonoBehaviour
{
    protected Rigidbody _rb;
    public float maxMoveSpeed = 5.0f;
    public float acceleration = 5.0f;
    [Range(0.0f, 1.0f)]
    public float friction = 0.5f;
    public bool DoMovement = false;
    [Min(0.0f)]
    public float popPathPointDistance = 0.5f;
    public bool doCheckForNearestPathPoint = true;

    public delegate void PathRethinkEvent();
    public event PathRethinkEvent OnPathComplete;
    public event PathRethinkEvent OnGiveUpPathing;

    public List<Vector3> pathToDestination = new List<Vector3>();

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(pathToDestination.Count <= 0 || !DoMovement)
        {
            Vector3 frictionVelocity = Vector3.Lerp(Vector3.zero, _rb.velocity, friction);
            frictionVelocity.y = _rb.velocity.y;
            _rb.velocity = frictionVelocity;
            return;
        }

        if (doCheckForNearestPathPoint)
        {
            float nearestSqrDistance = (transform.position - pathToDestination[0]).sqrMagnitude;
            for (int i = 1; i < pathToDestination.Count; i++)
            {
                float indexSqrDistance = (transform.position - pathToDestination[i]).sqrMagnitude;
                if(indexSqrDistance < nearestSqrDistance)
                {
                    //Remove all points that would have been before this point, as it's no longer necessary to move to them.
                    pathToDestination.RemoveRange(0, i);
                    i = 1;
                }
            }
        }

        //Part where we actually move the AI
        Vector3 moveDir = pathToDestination[0] - transform.position;
        while(moveDir.sqrMagnitude <= popPathPointDistance * popPathPointDistance && DoMovement)
        {
            pathToDestination.RemoveAt(0);
            if (pathToDestination.Count <= 0)
            {
                if (OnPathComplete != null) { OnPathComplete.Invoke(); }
                DoMovement = false;
            }
            else
            {
                moveDir = pathToDestination[0] - transform.position;
            }
        }

        moveDir.y = 0.0f;
        Vector3 newVelocity = _rb.velocity + (moveDir.normalized * acceleration);
        if(newVelocity.sqrMagnitude >= maxMoveSpeed * maxMoveSpeed)
        {
            newVelocity = newVelocity.normalized * maxMoveSpeed;
            newVelocity.y = _rb.velocity.y;
        }
        _rb.velocity = newVelocity;
    }
}
