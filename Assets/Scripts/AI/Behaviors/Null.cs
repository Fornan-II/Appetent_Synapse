using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class Null : AI.Behavior
{
    public override void OnEnter(AIController ai)
    {
        _currentPhase = StatePhase.ACTIVE;
    }

    public override void ActiveBehavior(AIController ai)
    {
        _currentPhase = StatePhase.EXITING;
    }

    public override void OnExit(AIController ai)
    {
        _currentPhase = StatePhase.INACTIVE;
    }
}