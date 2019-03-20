using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class StateMachine : MonoBehaviour
    {
        public Behavior CurrentState
        {
            get { return _activeState; }
        }
        protected Behavior _activeState;
        protected Behavior.StatePhase _oldStatePhase = Behavior.StatePhase.INACTIVE;

        public GameObject ReleventGameObject;

        protected virtual void FixedUpdate()
        {
            StateMachineProcessState();
        }

        protected virtual void StateMachineProcessState()
        {
            //Figure out what GameObject should be passed into state
            /*GameObject gameObjectToProccess = gameObject;
            if (ReleventGameObject)
            {
                gameObjectToProccess = ReleventGameObject;
            }

            //Start the queued state if active state is done.
            //if (_activeState == null && _queuedNextState != null)
            //{
            //    _activeState = _queuedNextState;
            //}

            //Run state behavior for _activeState
            if (_activeState != null)
            {
                Behavior.StatePhase phaseOnStateProcessing = _activeState.CurrentPhase;

                switch (_activeState.CurrentPhase)
                {
                    case Behavior.StatePhase.ENTERING:
                        {
                            if (_oldStatePhase != _activeState.CurrentPhase)
                            {
                                _activeState.OnEnter(gameObjectToProccess);
                            }
                            else
                            {
                                _activeState.EnterBehavior(gameObjectToProccess);
                            }
                            break;
                        }
                    case Behavior.StatePhase.ACTIVE:
                        {
                            _activeState.ActiveBehavior(gameObjectToProccess);
                            break;
                        }
                    case Behavior.StatePhase.EXITING:
                        {
                            if (_oldStatePhase != _activeState.CurrentPhase)
                            {
                                _activeState.OnExit(gameObjectToProccess);
                            }
                            else
                            {
                                _activeState.ExitBehavior(gameObjectToProccess);
                            }
                            break;
                        }
                    case Behavior.StatePhase.INACTIVE:
                        {
                            _activeState = null;
                            break;
                        }
                }

                _oldStatePhase = phaseOnStateProcessing;
            }*/
        }

        public void ForceNextState()
        {
            if (_activeState != null)
            {
                _activeState.ForceEndState();
            }
        }
    }
}