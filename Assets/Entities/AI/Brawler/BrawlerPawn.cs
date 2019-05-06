using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class BrawlerPawn : AIPawn
{
    public const string PROPERTY_ALERT = "DoAlert";

    public float AlertRadius = 32.0f;
    public LayerMask FriendlyAlertLayerMask = Physics.AllLayers;

    public override void Init(AIController controller)
    {
        base.Init(controller);
    }

    public virtual void AlertGroupMembers(Pawn instigator)
    {
        if (instigator.MyFaction != MyFaction)
        {
            Collider[] foundColliders = Physics.OverlapSphere(transform.position, AlertRadius, FriendlyAlertLayerMask);
            foreach(Collider c in foundColliders)
            {
                BrawlerPawn brawler = c.GetComponent<BrawlerPawn>();
                if (brawler)
                {
                    brawler.GiveAggro(instigator, 1);
                }
            }
        }
    }
}
