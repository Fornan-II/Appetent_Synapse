using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : RangedWeapon
{
    public GameObject ProjectilePrefab;
    public Transform barrel;
    public float projectileInitSpeed;

    public override bool UseSecondary(Pawn source)
    {
        return DoAttack(null, source);
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        Debug.Log("pew");

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
            Rigidbody pawnRB = user.GetComponent<Rigidbody>();
            Vector3 initVelocity = forward * projectileInitSpeed;
            if (pawnRB)
            {
                initVelocity += pawnRB.velocity;
            }
            proj.Initialize(initVelocity, damage, hittable, user, maxRange);
        }

        return true;
    }
}
