using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaceratorSword : MeleeWeapon
{
    public float SweepAttackRadius = 1.0f;
    public DamagePacket SweepAttackDamage = new DamagePacket(1, 3, DamagePacket.DamageType.GENERIC);

    protected bool _previousUsePrimary = false;

    //Future implementation: target is marked as target. Animation happens, and when OnTriggerEnter() intersects with the collider of target, that is when target takes damage.
    protected override void Use(Pawn user)
    {
        if (_anim)
        {
            // OH GOD PLEASE CHANGE THIS
            Vector3 vel = user.GetComponent<Rigidbody>().velocity;
            _anim.SetFloat("Speed", new Vector2(vel.x, vel.z).magnitude);
        }

        if(!(user is PlayerPawn))
        {
            return;
        }

        PlayerPawn playerUser = user as PlayerPawn;

        GameObject target = null;
        if(playerUser.Raycaster.RaycastInfo.DidHit)
        {
            target = playerUser.Raycaster.RaycastInfo.GetHitGameObjectAtRange(reach);
        }
        
        DoAttack(target, user);
    }

    public override bool DoAttack(GameObject target, Pawn user)
    {
        if (_anim)
        {
            _anim.SetTrigger("attack");
        }

        if(target)
        {
            RaycastHit? hitInfo = null;
            if (user is PlayerPawn)
            {
                PlayerPawn playerUser = user as PlayerPawn;
                hitInfo = playerUser.Raycaster.RaycastInfo.HitInfo;
            }

            DamageReciever.DealDamageToTarget(target, ScaleDamageByCharge(Damage), user, hitInfo);
            ResetAttackCharge();

            if (AttackCharge >= 0.848f)
            {
                Collider[] foundColliders = Physics.OverlapSphere(target.transform.position, SweepAttackRadius, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                foreach (Collider col in foundColliders)
                {
                    Pawn foundPawn = col.GetComponent<Pawn>();
                    if (foundPawn)
                    {
                        bool hitThisPawn = true;

                        if (user)
                        {
                            hitThisPawn = user.MyFaction != foundPawn.MyFaction;
                        }

                        if (hitThisPawn)
                        {
                            DamageReciever.DealDamageToTarget(target, SweepAttackDamage, user, hitInfo);
                        }
                    }
                }
            }

            return true;
        }
        
        ResetAttackCharge();
        return false;
    }
}
