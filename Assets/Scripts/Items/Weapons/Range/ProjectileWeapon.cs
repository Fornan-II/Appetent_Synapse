using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : RangedWeapon
{
    public GameObject ProjectilePrefab;
    public float projectileInitSpeed;

    protected bool _previousUsePrimary = false;

    protected override void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public override bool UsePrimary(Pawn source, bool value)
    {
        if (Ammo == 0)
        {
            return false;
        }
        //Else if ammo < 0 then player has "infinite" ammo

        _anim.SetBool("Charge", value);

        if(value)
        {
            if(_activeAttackChargeRoutine == null && AttackCharge < 1.0f)
            {
                ResetAttackCharge();
            }
        }
        else if(_previousUsePrimary)
        {
            if(_activeAttackChargeRoutine != null)
            {
                StopCoroutine(_activeAttackChargeRoutine);
                _activeAttackChargeRoutine = null;
            }

            _previousUsePrimary = value;
            return DoAttack(null, source);
        }

        _previousUsePrimary = value;
        return false;
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        if (Ammo > 0)
        {
            Ammo--;
        }

        if (!ProjectilePrefab)
        {
            Debug.LogWarning("No ProjectilePrefab assigned for " + name);
            return false;
        }

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
            //if (user)
            //{
            //    Rigidbody pawnRB = user.GetComponent<Rigidbody>();
            //    if (pawnRB)
            //    {
            //        initVelocity += pawnRB.velocity;
            //    }
            //}
            proj.Initialize(initVelocity, ScaleDamageByCharge(Damage), hittable, user, maxRange);
        }

        _attackCharge = 0.0f;

        return true;
    }

    public override void OnUnequip(Pawn source)
    {
        base.OnUnequip(source);
        _previousUsePrimary = false;
    }
}
