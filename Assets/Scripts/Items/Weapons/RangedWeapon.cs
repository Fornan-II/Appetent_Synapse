using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public DamagePacket damage = new DamagePacket(7, 5.0f);
    public float maxRange = 64.0f;
    public LayerMask hittable;

    protected Animator _anim;

    protected virtual void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        //Just basic hitscan
        if(_anim)
        {
            _anim.SetTrigger("attack");
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, maxRange, hittable, QueryTriggerInteraction.Ignore))
        {
            DamageReciever.DealDamageToTarget(hit.transform.gameObject, damage, user);
            return true;
        }
        
        return false;
    }
}
