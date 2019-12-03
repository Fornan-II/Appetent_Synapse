using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using AI.StateMachine;

public static partial class Behaviors
{
    public static readonly State Patrol = new State()
    {
        OnEnter = stateMachine =>
        {
            AIPawn aiPawn = stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn");
            if (aiPawn.moveScript)
            {
                stateMachine.Blackboard.SetProperty("howLongToWait", Random.Range(5.0f, 10.0f));
                stateMachine.Blackboard.SetProperty("doWander", false);
                aiPawn.moveScript.OnPathComplete += () => Patrol_GoGetPath(aiPawn.moveScript, stateMachine.Blackboard);
                aiPawn.moveScript.OnGiveUpPathing += () => Patrol_GoGetPath(aiPawn.moveScript, stateMachine.Blackboard);
                stateMachine.AdvancePhase();
            }
            else
            {
                stateMachine.ForceStateInactive();
            }
        },

        Active = stateMachine =>
        {
            //Decide between idling and wandering around
            if (!stateMachine.Blackboard.GetProperty<bool>("doWander"))
            {
                float howLongToWait = stateMachine.Blackboard.GetProperty<float>("howLongToWait");
                if (howLongToWait <= 0.0f)
                {
                    Patrol_GoGetPath(stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn").moveScript, stateMachine.Blackboard);
                }
                stateMachine.Blackboard.SetProperty("howLongToWait", howLongToWait - Time.fixedDeltaTime);
            }
            else if (stateMachine.Blackboard.GetProperty<bool>("newPathReady"))
            {
                stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn").moveScript.DoMovement = true;
                stateMachine.Blackboard.SetProperty("newPathReady", false);
            }
        },

        OnExit = stateMachine =>
        {
            stateMachine.Blackboard.RemoveProperty("howLongToWait");
            stateMachine.Blackboard.RemoveProperty("doWander");
            stateMachine.Blackboard.RemoveProperty("newPathReady");

            AIMoveScript moveScript = stateMachine.Blackboard.GetProperty<AIPawn>("aiPawn").moveScript;
            moveScript.OnPathComplete += () => Patrol_GoGetPath(moveScript, stateMachine.Blackboard);
            moveScript.OnGiveUpPathing += () => Patrol_GoGetPath(moveScript, stateMachine.Blackboard);

            stateMachine.AdvancePhase();
        }
    };

    private static void Patrol_GoGetPath(AIMoveScript movement, Blackboard bb)
    {
        float idleTime = Random.Range(0.0f, 10.0f);
        if (idleTime < 5.0f)
        {
            bb.SetProperty("doWander", true);
            movement.pathToDestination = new List<Vector3>(AI.Util.CalculatePath(movement.transform, AI.Util.GetRandomNearbyPoint(movement.transform.position, 15)));
            bb.SetProperty("newPathReady", true);
        }
        else
        {
            bb.SetProperty("newPathReady", false);
            bb.SetProperty("howLongToWait", idleTime);
        }
    }
}
