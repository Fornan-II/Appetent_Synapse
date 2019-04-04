using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class InstanceTest : Behavior
{
    string instanceName;

    public override void OnEnter(AIController ai)
    {
        instanceName = ai.name;
        _currentPhase = StatePhase.ACTIVE;
    }

    public override void ActiveBehavior(AIController ai)
    {
        ai.debugOutput = instanceName;
        _currentPhase = StatePhase.EXITING;
    }

    public override void OnExit(AIController ai)
    {
        _currentPhase = StatePhase.INACTIVE;
    }
}
