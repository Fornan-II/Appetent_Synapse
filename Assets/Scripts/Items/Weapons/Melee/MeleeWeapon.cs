using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float reach = 2.0f;

    protected Animator _anim;

    protected override void Start()
    {
        base.Start();
        _anim = gameObject.GetComponent<Animator>();
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        if (_anim)
        {
            _anim.SetTrigger("attack");
        }
        if (target)
        {
            DamageReciever.DealDamageToTarget(target, ScaleDamageByCharge(Damage), user);
            ResetAttackCharge();
            return true;
        }
        ResetAttackCharge();
        //If no target found to take damage, return false.
        return false;
    }
}
