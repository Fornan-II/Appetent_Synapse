using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public float maxRange = 64.0f;
    public LayerMask hittable;
    public Transform barrel;

    [SerializeField] private int _ammo = -1;
    public int Ammo
    {
        get { return _ammo; }
        set
        {
            if(value != _ammo)
            {
                _ammo = value;
                OnAmmoCountChange?.Invoke(_ammo);
            }
        }
    }

    public IntEvent OnAmmoCountChange;

    protected Animator _anim;

    protected override void Start()
    {
        base.Start();
        _anim = gameObject.GetComponent<Animator>();
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        if(Ammo > 0)
        {
            Ammo--;
        }
        else if(Ammo == 0)
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

        if (_ammo >= 0)
        {
            OnAmmoCountChange?.Invoke(_ammo);
        }
    }
}
