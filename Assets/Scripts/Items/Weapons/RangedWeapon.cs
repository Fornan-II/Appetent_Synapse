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
        //Physics.Raycast()
    }
}
