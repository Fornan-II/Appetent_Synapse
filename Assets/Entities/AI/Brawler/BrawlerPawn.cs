using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class BrawlerPawn : AIPawn
{
    public const string PROPERTY_ALERT = "DoAlert";

    public override void Init(AIController controller)
    {
        base.Init(controller);
    }

    public virtual void AlertGroupMembers(Pawn instigator)
    {
        if (_controller.MyGroup && instigator.MyFaction != MyFaction)
        {
            foreach (AIGroupMember groupMember in _controller.MyGroup.Members)
            {
                if (groupMember != _controller && groupMember is AIController)
                {
                    (groupMember as AIController).aiPawn.GiveAggro(instigator);
                }
            }
        }
    }
}
