﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class Null : AI.Behavior
{
    public override void OnEnter(Blackboard b)
    {
        _currentPhase = StatePhase.ACTIVE;
    }

    public override void ActiveBehavior(Blackboard b)
    {
        _currentPhase = StatePhase.EXITING;
    }

    public override void OnExit(Blackboard b)
    {
        _currentPhase = StatePhase.INACTIVE;
    }
}