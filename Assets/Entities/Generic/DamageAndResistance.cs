using UnityEngine;

#region Float Packets
[System.Serializable]
public struct DamagePacket
{
    public enum DamageType
    {
        GENERIC,
        PROJECTILE,
        STRUCTURAL
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
    [Range(0.0f, 1.0f)]
    public float StructuralResistance;

    public Resistance(float generic, float projectile, float knockback, float structural)
    {
        GenericResistance = generic;
        ProjectileResistance = projectile;
        KnockbackResistance = knockback;
        StructuralResistance = structural;
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
    public Modifier StructuralResistance;

    public ModifierResistance(Modifier generic, Modifier projectile, Modifier knockback, Modifier structural)
    {
        GenericResistance = generic;
        ProjectileResistance = projectile;
        KnockbackResistance = knockback;
        StructuralResistance = structural;
    }

    public ModifierResistance(float generic, float projectile, float knockback, float structural)
    {
        GenericResistance = new Modifier(generic, Modifier.CalculateMode.ADD);
        ProjectileResistance = new Modifier(projectile, Modifier.CalculateMode.ADD);
        KnockbackResistance = new Modifier(knockback, Modifier.CalculateMode.ADD);
        StructuralResistance = new Modifier(structural, Modifier.CalculateMode.ADD);
    }

    public ModifierResistance(Resistance resistance)
    {
        GenericResistance = new Modifier(resistance.GenericResistance, Modifier.CalculateMode.ADD);
        ProjectileResistance = new Modifier(resistance.ProjectileResistance, Modifier.CalculateMode.ADD);
        KnockbackResistance = new Modifier(resistance.KnockbackResistance, Modifier.CalculateMode.ADD);
        StructuralResistance = new Modifier(resistance.StructuralResistance, Modifier.CalculateMode.ADD);
    }
}

#endregion