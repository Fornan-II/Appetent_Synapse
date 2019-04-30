using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : RangedWeapon
{
    public GameObject ProjectilePrefab;
    public float projectileInitSpeed;

    public override bool UseSecondary(Pawn source)
    {
        return DoAttack(null, source);
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        if (!ProjectilePrefab)
        {
            Debug.LogWarning("No ProjectilePrefab assigned for " + name);
            return false;
        }

        base.DoAttack(target, user);

        GameObject spawnedProj;
        Vector3 forward;
        if (barrel)
        {
            spawnedProj = Instantiate(ProjectilePrefab, barrel.position, barrel.rotation);
            forward = barrel.forward;
        }
        else
        {
            spawnedProj = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
            forward = transform.forward;
        }

        Projectile proj = spawnedProj.GetComponent<Projectile>();
        if(proj)
        {
            Vector3 initVelocity = forward * projectileInitSpeed * _attackCharge;
            if (user)
            {
                Rigidbody pawnRB = user.GetComponent<Rigidbody>();
                if (pawnRB)
                {
                    initVelocity += pawnRB.velocity;
                }
            }
            proj.Initialize(initVelocity, ScaleDamageByCharge(Damage), hittable, user, maxRange);
        }

        ResetAttackCharge();

        return true;
    }
}
