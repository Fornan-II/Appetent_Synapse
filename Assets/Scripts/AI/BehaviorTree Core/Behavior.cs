using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Behavior : ScriptableObject
    {
        public enum StatePhase
        {
            ENTERING,
            ACTIVE,
            EXITING,
            INACTIVE
        }

        protected void OnEnable()
        {
            //hideFlags = HideFlags.HideAndDontSave;
        }

        protected StatePhase _currentPhase = StatePhase.ENTERING;
        public StatePhase CurrentPhase { get { return _currentPhase; } }

        public abstract void OnEnter(AIController ai);

        public abstract void ActiveBehavior(AIController ai);

        public abstract void OnExit(AIController ai);

        public virtual void EnterBehavior(AIController ai) { }

        public virtual void ExitBehavior(AIController ai) { }

        public virtual void ForceEndState()
        {
            _currentPhase = StatePhase.EXITING;
        }
    }
}