using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : EquippedHoldableItem
{
    public DamagePacket Damage;
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
        dmg.HitPoints = (int)(dmg.HitPoints * _attackCharge);
        return dmg;
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
}
