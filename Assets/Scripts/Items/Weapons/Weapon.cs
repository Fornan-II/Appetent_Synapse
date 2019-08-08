using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Weapon : EquippedHoldableItem
{
    public ModifierDamagePacket Damage;
    public float AttackSpeed = 0.6f;
    [SerializeField]protected float _attackCharge = 0.0f;
    public float AttackCharge { get { return _attackCharge; } }
    protected Coroutine _activeAttackChargeRoutine;

    protected virtual void Start()
    {
        ResetAttackCharge();
    }

    public abstract bool DoAttack(GameObject target, Pawn user);

    protected virtual DamagePacket ScaleDamageByCharge(DamagePacket dmg)
    {
        dmg.HitPoints = Mathf.FloorToInt(dmg.HitPoints * (0.2f + _attackCharge * _attackCharge * 0.8f));
        dmg.Knockback = dmg.Knockback * _attackCharge;

        return dmg;
    }

    protected virtual DamagePacket ScaleDamageByCharge(ModifierDamagePacket dmg)
    {
        int hp = Mathf.FloorToInt(dmg.HitPoints.Value * (0.2f + _attackCharge * _attackCharge * 0.8f));
        float kb = dmg.Knockback.Value * _attackCharge;

        return new DamagePacket(hp, kb, dmg.Type);
    }

    protected virtual void ResetAttackCharge()
    {
        if (AttackSpeed > 0)
        {
            if (_activeAttackChargeRoutine != null)
            {
                StopCoroutine(_activeAttackChargeRoutine);
            }

            _activeAttackChargeRoutine = StartCoroutine(RunAttackCharge());
        }
        else
        {
            _attackCharge = 1.0f;
        }
    }

    protected virtual IEnumerator RunAttackCharge()
    {
        _attackCharge = 0.0f;
        
        for(float timer = 0.0f; timer < AttackSpeed; timer += Time.deltaTime)
        {
            _attackCharge = Mathf.Clamp01(timer / AttackSpeed);
            yield return null;
        }

        _attackCharge = 1.0f;
        _activeAttackChargeRoutine = null;
    }

    public override void OnEquip(Pawn source)
    {
        base.OnEquip(source);
        ResetAttackCharge();
    }
}
