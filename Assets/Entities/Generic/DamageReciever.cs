﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamagePacket
{
    public enum DamageType
    {
        GENERIC,
        PROJECTILE
    }

    public int HitPoints;
    public float Knockback;
    public DamageType Type;

    public DamagePacket(int hp, float kb, DamageType type)
    {
        HitPoints = hp;
        Knockback = kb;
        Type = type;
    }

    public DamagePacket(ModifierDamagePacket packet)
    {
        HitPoints = Mathf.FloorToInt(packet.HitPoints.Value);
        Knockback = packet.HitPoints.Value;
        Type = packet.Type;
    }
}

[System.Serializable]
public struct Resistance
{
    [Range(0.0f, 1.0f)]
    public float GenericResistance;
    [Range(0.0f, 1.0f)]
    public float ProjectileResistance;
    [Range(0.0f, 1.0f)]
    public float KnockbackResistance;

    public Resistance(float generic, float projectile, float knockback)
    {
        GenericResistance = generic;
        ProjectileResistance = projectile;
        KnockbackResistance = knockback;
    }
}

public class DamageReciever : MonoBehaviour
{
    protected const float verticalKnockbackScalar = 0.7f;
    protected const float IFrameDuration = 0.5f;

    [SerializeField]protected int _health = 20;
    public int MaxHealth = 20;
    public bool IgnoreDamage = false;

    public Resistance Resistances = new Resistance(0, 0, 0);

    public GameObject HitParticles;

    public PawnEvent OnDamageTaken;
    public PawnEvent OnKilled;
    public IntEvent OnHealthValueChanged;

    protected Coroutine _IFrameRoutine = null;
    protected int _dmgTakenForIFrames = 0;

    public virtual void TakeDamage(int hitPoints, Pawn source)
    {
        if(IgnoreDamage)
        {
            return;
        }

        OnDamageTaken.Invoke(source);
        if(_health - hitPoints <= 0)
        {
            AddHealth(-hitPoints);
            if (_IFrameRoutine != null)
            {
                StopCoroutine(_IFrameRoutine);
            }
            _IFrameRoutine = null;

            Die(source);
        }
        else if(_IFrameRoutine == null)
        {
            _dmgTakenForIFrames = hitPoints;
            AddHealth(-hitPoints);
            _IFrameRoutine = StartCoroutine(RunIFrames());
        }
        else if(_dmgTakenForIFrames < hitPoints)
        {
            AddHealth(_dmgTakenForIFrames - hitPoints);
            _dmgTakenForIFrames = hitPoints;
        }
    }

    public virtual void AddHealth(int value)
    {
        if(value != 0)
        {
            _health += value;
            _health = Mathf.Clamp(_health, 0, MaxHealth);

            OnHealthValueChanged.Invoke(_health);
        }
    }

    public virtual int CalculateDamage(DamagePacket dmg)
    {
        float finalDamage = dmg.HitPoints;

        switch(dmg.Type)
        {
            case DamagePacket.DamageType.GENERIC:
                {
                    finalDamage = finalDamage * (1.0f - Resistances.GenericResistance);
                    break;
                }
            case DamagePacket.DamageType.PROJECTILE:
                {
                    finalDamage = finalDamage * (1.0f - Resistances.ProjectileResistance);
                    break;
                }
        }

        return Mathf.Max(0, (int)finalDamage);
    }

    public virtual void Die(Pawn killer)
    {
        if(IgnoreDamage)
        {
            return;
        }

        if(killer)
        {
            killer.OnKill(this);
        }
        OnKilled.Invoke(killer);
        IgnoreDamage = true;
    }

    protected IEnumerator RunIFrames()
    {
        yield return new WaitForSeconds(IFrameDuration);
        _dmgTakenForIFrames = 0;
        _IFrameRoutine = null;
    }

    public static void DealDamageToTarget(GameObject target, DamagePacket dmg, Pawn source, RaycastHit? hitInfo = null)
    {
        DamageReciever targetDR = target.GetComponent<DamageReciever>();
        float kbRes = 0.0f;
        if(targetDR)
        {
            targetDR.TakeDamage(targetDR.CalculateDamage(dmg), source);
            kbRes = targetDR.Resistances.KnockbackResistance;

            if(targetDR.HitParticles && hitInfo.HasValue)
            {
                GameObject spawnedParticles = Instantiate(targetDR.HitParticles, hitInfo.Value.point, Quaternion.identity);
                spawnedParticles.transform.forward = hitInfo.Value.normal;
            }
        }

        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        if(targetRB)
        {
            float finalKnockback = Mathf.Max(0.0f, dmg.Knockback * (1.0f - kbRes));
            Vector3 knockbackVector = (targetRB.transform.position - source.transform.position).normalized * finalKnockback;
            knockbackVector.y = finalKnockback * verticalKnockbackScalar;
            if (hitInfo.HasValue)
            {
                targetRB.AddForceAtPosition(knockbackVector, hitInfo.Value.point, ForceMode.Impulse);
            }
            else
            {
                targetRB.AddForce(knockbackVector, ForceMode.Impulse);
            }
        }
    }
}
