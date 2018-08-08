using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaceratorSword : Item
{

    public float damage = 10.0f;
    public float reach = 2.0f;
    public float sensitivity = 0.1f;

    protected Animator _anim;

    protected virtual void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if(_wielder)
        {
            Rigidbody rb = _wielder.GetComponent<Rigidbody>();
            if (rb && _anim)
            {
                Vector2 XZplaneVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
                _anim.SetFloat("velocity", XZplaneVelocity.sqrMagnitude);
            }
        }
    }

    protected override bool ProcessInteraction(Actor source)
    {
        if (base.ProcessInteraction(source))
        {
            if (_anim)
            {
                _anim.SetBool("beingHeld", true);
            }
            return true;
        }
        return false;
    }

    //Future implementation: target is marked as target. Animation happens, and when OnTriggerEnter() intersects with the collider of target, that is when target takes damage.
    public override bool Use(Actor user)
    {
        Game_Pawn GP = (Game_Pawn)user;
        GameObject target = GP.GetInteractableObject(reach, sensitivity);
        base.Use(user);
        if(_anim)
        {
            _anim.SetTrigger("attack");
        }
        if (target)
        {
            Actor targetActor = target.GetComponent<Actor>();
            if (targetActor)
            {
                targetActor.TakeDamage(user, damage);
                return true;
            }
        }
        //If no actor found to take damage, return false.
        return false;
    }

    public override void SetNotBeingHeld()
    {
        base.SetNotBeingHeld();
        if(_anim)
        {
            _anim.SetBool("beingHeld", false);
        }
    }
}
