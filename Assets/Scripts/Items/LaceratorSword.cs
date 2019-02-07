using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaceratorSword : EquippedHoldableItem
{
    public DamagePacket damage = new DamagePacket(10, 10.0f);
    public float reach = 2.0f;
    public float sensitivity = 0.1f;

    protected Animator _anim;

    protected virtual void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    //Future implementation: target is marked as target. Animation happens, and when OnTriggerEnter() intersects with the collider of target, that is when target takes damage.
    public override bool UsePrimary(Pawn user)
    {
        GameObject target = user.MyInteracter.GetInteractableObject(reach, sensitivity);

        if (_anim)
        {
            _anim.SetTrigger("attack");
        }
        if (target)
        {
            DamageReciever.DealDamageToTarget(target, damage, user);
        }
        //If no target found to take damage, return false.
        return false;
    }
}
