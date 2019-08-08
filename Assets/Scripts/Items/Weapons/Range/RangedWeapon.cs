using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public float maxRange = 64.0f;
    public LayerMask hittable;
    public Transform barrel;

    public int ammo = -1;

    protected Animator _anim;

    protected override void Start()
    {
        base.Start();
        _anim = gameObject.GetComponent<Animator>();
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        if(ammo > 0)
        {
            ammo--;
        }
        else if(ammo == 0)
        {
            return false;
        }
        //Else if ammo < 0 then player has "infinite" ammo

        //Just basic hitscan
        if(_anim)
        {
            _anim.SetTrigger("attack");
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, maxRange, hittable, QueryTriggerInteraction.Ignore))
        {
            DamageReciever.DealDamageToTarget(hit.transform.gameObject, ScaleDamageByCharge(Damage), user, hit);
            return true;
        }
        
        return false;
    }

    public override void OnEquip(Pawn source)
    {
        if((source.defaultBarrel && !barrel) || source.overrideBarrel)
        {
            barrel = source.defaultBarrel;
        }
    }
}
