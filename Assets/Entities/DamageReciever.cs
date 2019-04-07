using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageEvent : UnityEvent<Pawn> { }

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

    public DamageEvent OnDamageTaken;
    public DamageEvent OnKilled;

    private void Start()
    {

    }

    public virtual void TakeDamage(float hitPoints, Pawn source)
    {
        //Damage
        int finalHitPoints = Mathf.Max(0, (int)(hitPoints * (1.0f - DamageResistance)));
        OnDamageTaken.Invoke(source);
        Health -= finalHitPoints;
        if(Health <= 0)
        {
            Die(source);
        }
    }

    public virtual void Die(Pawn killer)
    {
        //Temp, do something better here eventually.
        OnKilled.Invoke(killer);
        Destroy(gameObject);
    }

    public static void DealDamageToTarget(GameObject target, DamagePacket dmg, Pawn source)
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
            targetRB.AddForce(knockbackVector, ForceMode.Impulse);
        }
    }
}
