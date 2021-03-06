﻿using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class Lancer : AIPawn
{
    protected Rigidbody _rb;
    public float ShootComfortableRange = 15.0f;
    public float ShootMaxRange = 25.0f;
    public float Innacuracy = 1.0f;

    public const string PROPERTY_INRANGE = "InRange";

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void Init(AIController controller)
    {
        base.Init(controller);
        controller.localBlackboard.SetProperty(MoveToTarget.PROPERTY_DESIREDTARGETDISTANCE, ShootComfortableRange);
    }

    public virtual void AimAt(Transform target)
    {
        ProjectileWeapon weapon = null;
        if (equippedWeapon is ProjectileWeapon)
        {
            weapon = equippedWeapon as ProjectileWeapon;
        }

        Vector3 bodyVector = target.position;
        bodyVector.y = transform.position.y;

        Vector3 targetPos = target.position;

        if (weapon)
        {
            //Figure out how long the projectile will be in the air
            Vector3 aimVector = targetPos;
            aimVector -= weapon.barrel.position;
            aimVector -= Vector3.Project(aimVector, Physics.gravity);
            float travelTime = aimVector.magnitude / weapon.projectileInitSpeed;

            //Account for my velocity
            if (_rb)
            {
                targetPos += _rb.velocity * travelTime;
            }

            //Account for target velocity
            Rigidbody targetRB = target.GetComponent<Rigidbody>();
            if (targetRB)
            {
                targetPos += _rb.velocity * travelTime;
            }

            //Account for gravity
            targetPos -= Physics.gravity * (travelTime * travelTime);
        }

        //Aim things correctly
        transform.LookAt(bodyVector);
        equippedWeapon.transform.LookAt(targetPos);
        if (weapon)
        {
            weapon.barrel.LookAt(targetPos + Random.onUnitSphere * Innacuracy);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_controller)
        {
            Pawn target = _controller.localBlackboard.GetProperty<Pawn>("target");
            if (target)
            {
                float sqrDistance = (transform.position - target.transform.position).sqrMagnitude;
                bool value = ShootMaxRange * ShootMaxRange > sqrDistance;
                _controller.localBlackboard.SetProperty(PROPERTY_INRANGE, value);
            }
        }
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if(_controller)
        {
            _controller.localBlackboard.SetProperty(MoveToTarget.PROPERTY_DESIREDTARGETDISTANCE, ShootComfortableRange);
        }
    }
#endif
}
