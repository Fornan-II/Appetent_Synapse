using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.StateMachine
{
    public class StateMachine
    {
        protected enum StatePhase
        {
            ENTERING,
            ACTIVE,
            EXITING,
            INACTIVE
        }

        public State CurrentState { get; protected set; }
        public Blackboard Blackboard;
        protected StatePhase _currentStatePhase;
        protected StatePhase _previousStatePhase;
        public float DeltaTime { get; private set; } = Mathf.Infinity;

        public StateMachine()
        {
            Blackboard = new Blackboard();
            _currentStatePhase = StatePhase.INACTIVE;
            _previousStatePhase = StatePhase.INACTIVE;
        }

        public virtual void AdvancePhase(int iterations = 1)
        {
            _currentStatePhase = (StatePhase)Mathf.Clamp((int)(_currentStatePhase + iterations), 0, 3);
        }

        public virtual void Process(float deltaTime)
        {
            DeltaTime = deltaTime;

            if (CurrentState == null)
                return;

            //Save a version of the state in case it is changed within any of the 
            StatePhase stateOnProcessing = _currentStatePhase;

            switch (_currentStatePhase)
            {
                case StatePhase.ENTERING:
                    {
                        if (_previousStatePhase != stateOnProcessing)
                        {
                            CurrentState.OnEnter?.Invoke(this);
                        }
                        CurrentState.Entering?.Invoke(this);
                        break;
                    }
                case StatePhase.ACTIVE:
                    {
                        if (_previousStatePhase != stateOnProcessing)
                        {
                            CurrentState.OnActive?.Invoke(this);
                        }
                        CurrentState.Active?.Invoke(this);
                        break;
                    }
                case StatePhase.EXITING:
                    {
                        if (_previousStatePhase != stateOnProcessing)
                        {
                            CurrentState.OnExit?.Invoke(this);
                        }
                        CurrentState.Exiting?.Invoke(this);
                        break;
                    }
                case StatePhase.INACTIVE:
                    {
                        CurrentState = null;
                        _previousStatePhase = StatePhase.INACTIVE;
                        return;
                    }
            }

            _previousStatePhase = stateOnProcessing;
        }

        public virtual void ForceStateInactive()
        {
            _currentStatePhase = StatePhase.INACTIVE;
        }

        public virtual void ForceEndState()
        {
            if (CurrentState != null && !(_currentStatePhase == StatePhase.EXITING || _currentStatePhase == StatePhase.INACTIVE))
            {
                _currentStatePhase = StatePhase.EXITING;
            }
        }

        public int GetPhase()
        {
            return (int)_currentStatePhase;
        }
    }
}