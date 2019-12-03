using AI.StateMachine;

public static partial class Behaviors
{
    public static readonly State RangedAttack = new State()
    {
        Label = "RangedAttack",
        OnEnter = stateMachine =>
        {
            Pawn target = stateMachine.Blackboard.GetProperty<Pawn>("target");
            Lancer lancerPawn = stateMachine.Blackboard.GetProperty<Lancer>("aiPawn");
            if (target && lancerPawn && lancerPawn.equippedWeapon)
            {
                stateMachine.AdvancePhase();
            }
            else
            {
                stateMachine.ForceStateInactive();
            }
        },

        Active = stateMachine =>
        {
            if (!(stateMachine.Blackboard.GetProperty<bool>(Lancer.PROPERTY_INRANGE) && stateMachine.Blackboard.GetProperty<bool>(AIPawn.PROPERTY_AGGRO)))
            {
                stateMachine.ForceStateInactive();
            }
            else
            {
                Pawn target = stateMachine.Blackboard.GetProperty<Pawn>("target");
                Lancer lancerPawn = stateMachine.Blackboard.GetProperty<Lancer>("aiPawn");
                if (target)
                {
                    lancerPawn.AimAt(target.transform);
                    bool notReadyToFire = lancerPawn.equippedWeapon.AttackCharge < 1.0f;
                    lancerPawn.equippedWeapon.UseItem(lancerPawn, notReadyToFire);
                }
                else
                {
                    stateMachine.ForceStateInactive();
                }
            }
        },

        OnExit = stateMachine => stateMachine.ForceStateInactive()
    };
}