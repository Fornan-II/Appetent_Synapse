using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public DamagePacket damage = new DamagePacket(10, 10.0f);
    public float reach = 2.0f;

    protected Animator _anim;

    protected virtual void Start()
    {
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
            if((target.transform.position - user.transform.position).sqrMagnitude <= reach * reach)
            {
                DamageReciever.DealDamageToTarget(target, damage, user);
                return true;
            }
        }
        //If no target found to take damage, return false.
        return false;
    }
}
