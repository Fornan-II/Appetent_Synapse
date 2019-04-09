using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class AlertGroup : Behavior
{
    public override void OnEnter(AIController ai)
    {
        DoAlert(ai);
        _currentPhase = StatePhase.INACTIVE;
    }

    public override void ActiveBehavior(AIController ai)
    {
        DoAlert(ai);
        _currentPhase = StatePhase.INACTIVE;
    }

    public override void OnExit(AIController ai)
    {
        DoAlert(ai);
        _currentPhase = StatePhase.INACTIVE;
    }

    protected virtual void DoAlert(AIController ai)
    {
        object objLocalGroup = ai.localBlackboard.GetProperty("localGroup");
        object objTarget = ai.localBlackboard.GetProperty("target");
        if (objLocalGroup is List<AIController> && objTarget is Pawn)
        {
            foreach (AIController groupMember in objLocalGroup as List<AIController>)
            {
                if (groupMember.aiPawn is BrawlerPawn && groupMember != ai)
                {
                    BrawlerPawn brawler = groupMember.aiPawn as BrawlerPawn;
                    groupMember.localBlackboard.SetProperty(BrawlerPawn.PROPERTY_ALERT, false);
                    brawler.GiveAggro(objTarget as Pawn);
                }
            }
        }
    }
}
