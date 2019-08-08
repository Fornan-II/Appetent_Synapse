using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Float Packets
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
#endregion

#region Modifier Packets
[System.Serializable]
public struct ModifierDamagePacket
{
    public Modifier HitPoints;
    public Modifier Knockback;
    public DamagePacket.DamageType Type;

    public ModifierDamagePacket(Modifier hp, Modifier kb, DamagePacket.DamageType type)
    {
        HitPoints = hp;
        Knockback = kb;
        Type = type;
    }

    public ModifierDamagePacket(int hp, float kb, DamagePacket.DamageType type)
    {
        HitPoints = new Modifier(hp);
        Knockback = new Modifier(kb);
        Type = type;
    }

    public ModifierDamagePacket(DamagePacket packet)
    {
        HitPoints = new Modifier(packet.HitPoints);
        Knockback = new Modifier(packet.Knockback);
        Type = packet.Type;
    }
}

[System.Serializable]
public struct ModifierResistance
{
    public Modifier GenericResistance;
    public Modifier ProjectileResistance;
    public Modifier KnockbackResistance;

    public ModifierResistance(Modifier generic, Modifier projectile, Modifier knockback)
    {
        GenericResistance = generic;
        ProjectileResistance = projectile;
        KnockbackResistance = knockback;
    }

    public ModifierResistance(float generic, float projectile, float knockback)
    {
        GenericResistance = new Modifier(generic);
        ProjectileResistance = new Modifier(projectile);
        KnockbackResistance = new Modifier(knockback);
    }

    public ModifierResistance(Resistance resistance)
    {
        GenericResistance = new Modifier(resistance.GenericResistance);
        ProjectileResistance = new Modifier(resistance.ProjectileResistance);
        KnockbackResistance = new Modifier(resistance.KnockbackResistance);
    }
}

#endregion