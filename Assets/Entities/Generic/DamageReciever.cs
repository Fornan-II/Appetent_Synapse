using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamagePacket
{
    public int HitPoints;
    public float Knockback;
    public DamagePacket(int hp, float kb)
    {
        HitPoints = hp;
        Knockback = kb;
    }
}

public class DamageReciever : MonoBehaviour
{
    public int Health = 20;
    public int MaxHealth = 20;
    [Range(0.0f, 1.0f)]
    public float DamageResistance = 0.0f;

    [Range(0.0f, 1.0f)]
    public float KnockbackResistance = 0.0f;

    public PawnEvent OnDamageTaken;
    public PawnEvent OnKilled;

    public virtual void TakeDamage(float hitPoints, Pawn source)
    {
        //Damage
        int finalHitPoints = Mathf.Max(0, (int)(hitPoints * (1.0f - DamageResistance)));
        OnDamageTaken.Invoke(source);
        AddHealth(-finalHitPoints);
        if(Health <= 0)
        {
            Die(source);
        }
    }

    public virtual void AddHealth(int value)
    {
        Health += value;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
    }

    public virtual void Die(Pawn killer)
    {
        killer.OnKill(this);
        OnKilled.Invoke(killer);
    }

    public static void DealDamageToTarget(GameObject target, DamagePacket dmg, Pawn source, RaycastHit? hitInfo = null)
    {
        DamageReciever targetDR = target.GetComponent<DamageReciever>();
        float kbRes = 0.0f;
        if(targetDR)
        {
            targetDR.TakeDamage(dmg.HitPoints, source);
            kbRes = targetDR.KnockbackResistance;
        }

        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        if(targetRB)
        {
            float finalKnockback = Mathf.Max(0.0f, dmg.Knockback * (1.0f - kbRes));
            Vector3 knockbackVector = (targetRB.transform.position - source.transform.position).normalized * finalKnockback;
            knockbackVector.y = finalKnockback * 0.7f;
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

    public virtual void Reset()
    {
        Health = MaxHealth;
    }
}
