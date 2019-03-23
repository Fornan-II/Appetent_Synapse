using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Behavior
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

        public virtual void OnEnter(Blackboard b)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ActiveBehavior(Blackboard b)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnExit(Blackboard b)
        {
            throw new System.NotImplementedException();
        }

        public virtual void EnterBehavior(Blackboard b) { }

        public virtual void ExitBehavior(Blackboard b) { }

        public virtual void ForceEndState()
        {
            _currentPhase = StatePhase.EXITING;
        }
    }
}