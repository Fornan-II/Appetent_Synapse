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
    public bool DoFriction = true;
    [Min(0.0f)]
    public Vector3 footPositionOffset = Vector3.zero;
    public float popPathPointDistance = 0.1f;
    public float PathingGiveUpTime = 7.0f;
    protected float _remainingGiveUpTime = 0.0f;

    public delegate void PathRethinkEvent();
    public event PathRethinkEvent OnPathComplete;
    public event PathRethinkEvent OnGiveUpPathing;

    public List<Vector3> pathToDestination = new List<Vector3>();

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _remainingGiveUpTime = PathingGiveUpTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 footPos = transform.position + footPositionOffset;

        if (pathToDestination.Count <= 0 || !DoMovement)
        {
            if (DoFriction)
            {
                Vector3 frictionVelocity = Vector3.Lerp(Vector3.zero, _rb.velocity, friction);
                frictionVelocity.y = _rb.velocity.y;
                _rb.velocity = frictionVelocity;
            }
            return;
        }

        //Part where we actually move the AI
        Vector3 moveDir = pathToDestination[0] - footPos;
        while(moveDir.sqrMagnitude <= popPathPointDistance * popPathPointDistance && DoMovement)
        {
            pathToDestination.RemoveAt(0);
            _remainingGiveUpTime = PathingGiveUpTime;
            if (pathToDestination.Count <= 0)
            {
                if (OnPathComplete != null) { OnPathComplete.Invoke(); }
                DoMovement = false;
            }
            else
            {
                moveDir = pathToDestination[0] - footPos;
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

        if(_remainingGiveUpTime <= 0.0f)
        {
            _remainingGiveUpTime = PathingGiveUpTime;
            pathToDestination.Clear();
            DoMovement = false;
            Debug.Log(name + " says: \"I give up!\"");
            if(OnGiveUpPathing != null) { OnGiveUpPathing.Invoke(); }
        }
    }

    protected Coroutine activeStunRoutine;
    public virtual void GetStunned(Pawn source)
    {
        if(activeStunRoutine != null)
        {
            StopCoroutine(activeStunRoutine);
        }

        activeStunRoutine = StartCoroutine(StunTimer());
    }

    protected virtual IEnumerator StunTimer()
    {
        bool tempFric = DoFriction;
        bool tempMove = DoMovement;
        DoFriction = false;
        DoMovement = false;

        yield return new WaitForSeconds(1.0f);

        DoFriction = tempFric;
        DoMovement = tempMove;
        activeStunRoutine = null;
    }

    protected virtual void OnDrawGizmos()
    {
        Vector3 footPos = transform.position + footPositionOffset;

        Gizmos.DrawLine(footPos + Vector3.forward * 0.3f, footPos - Vector3.forward * 0.3f);
        Gizmos.DrawLine(footPos + Vector3.right * 0.3f, footPos - Vector3.right * 0.3f);
    }
}
