using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class RangedAttack : AI.Behavior
{
    protected Pawn target;
    protected Lancer lancerPawn;

    protected const float recalculatePathDistance = 5.0f;

    public override void OnEnter(AIController ai)
    {
        target = ai.localBlackboard.GetProperty<Pawn>("target");
        if (target && ai.aiPawn is Lancer && ai.aiPawn.equippedWeapon)
        {
            lancerPawn = ai.aiPawn as Lancer;

            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.INACTIVE;
        }
    }

    public override void ActiveBehavior(AIController ai)
    {
        
        if(!(ai.localBlackboard.GetProperty<bool>(Lancer.PROPERTY_INRANGE) && ai.localBlackboard.GetProperty<bool>(AIPawn.PROPERTY_AGGRO)))
        {
            _currentPhase = StatePhase.INACTIVE;
        }
        else if(target)
        {
            lancerPawn.AimAt(target.transform);
            //lancerPawn.equippedWeapon.DoAttack(target.gameObject, lancerPawn);
            bool notReadyToFire = lancerPawn.equippedWeapon.AttackCharge < 1.0f;
            lancerPawn.equippedWeapon.UseItem(lancerPawn, notReadyToFire);
        }
        else
        {
            _currentPhase = StatePhase.INACTIVE;
        }
    }

    public override void OnExit(AIController ai)
    {
        _currentPhase = StatePhase.INACTIVE;
    }
}