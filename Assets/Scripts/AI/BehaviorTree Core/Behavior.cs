using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Behavior
    {
        public enum StatePhase
        {
            ENTERING,
            ACTIVE,
            EXITING,
            INACTIVE
        }

        protected StatePhase _currentPhase = StatePhase.ENTERING;
        public StatePhase CurrentPhase { get { return _currentPhase; } }

        public abstract void OnEnter(Blackboard b);

        public abstract void ActiveBehavior(Blackboard b);

        public abstract void OnExit(Blackboard b);

        public virtual void EnterBehavior(Blackboard b) { }

        public virtual void ExitBehavior(Blackboard b) { }

        public virtual void ForceEndState()
        {
            _currentPhase = StatePhase.EXITING;
        }
    }
}