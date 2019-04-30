using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : MonoBehaviour
{
    public ProjectileWeapon weapon;
    protected Rigidbody _rb;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void AimAt(Transform target)
    {
        Vector3 bodyVector = target.position;
        bodyVector.y = transform.position.y;

        DrawPoint(target.position);
        Vector3 targetPos = target.position;

        //Account for gravity
        Vector3 aimVector = targetPos - weapon.barrel.position;
        aimVector -= Vector3.Project(aimVector, Physics.gravity);
        float travelTime = aimVector.magnitude / weapon.projectileInitSpeed;

        //Account for my velocity;
        if(_rb)
        {
            targetPos += _rb.velocity * travelTime;
        }

        //Account for target velocity
        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        if(targetRB)
        {
            targetPos += _rb.velocity * travelTime;
        }

        targetPos -= Physics.gravity * (travelTime * travelTime);

        DrawPoint(targetPos, Color.blue);
        Debug.DrawLine(target.position, targetPos);

        //Aim things correctly
        transform.LookAt(bodyVector);
        weapon.transform.LookAt(targetPos);
        weapon.barrel.LookAt(targetPos);
    }


    //EVERYTHING BELOW HERE IS FOR DEBUG PORPOISES
    //
    public bool doTheShoots = false;
    public Transform t;
    protected virtual void Update()
    {
        if(t)
        {
            AimAt(t);
        }

        if(doTheShoots)
        {
            doTheShoots = false;
            weapon.DoAttack(null, null);
        }
    }

    void DrawPoint(Vector3 position)
    {
        DrawPoint(position, Color.white);
    }

    void DrawPoint(Vector3 position, Color c)
    {
        Debug.DrawRay(position - Vector3.up * 0.25f, Vector3.up * 0.5f, c);
        Debug.DrawRay(position - Vector3.right * 0.25f, Vector3.right * 0.5f, c);
        Debug.DrawRay(position - Vector3.forward * 0.25f, Vector3.forward * 0.5f, c);
    }
}
