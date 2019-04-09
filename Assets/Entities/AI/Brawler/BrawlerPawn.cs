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
        _controller.localBlackboard.SetProperty(PROPERTY_ALERT, true);
    }
}
