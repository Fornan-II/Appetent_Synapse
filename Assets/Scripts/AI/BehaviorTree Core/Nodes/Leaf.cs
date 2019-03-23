using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Leaf 1", menuName = "BehaviorTree/Nodes/New Leaf")]
    public class Leaf : Node
    {
        [SerializeField]protected Behavior _nodeBehavior;
        public Behavior NodeBehavior
        {
            get
            {
                return _nodeBehavior;
            }
            set
            {
                if(UnityEditor.AssetDatabase.Contains(this))
                {
                    if (_nodeBehavior)
                    {
                        UnityEditor.AssetDatabase.RemoveObjectFromAsset(_nodeBehavior);
                    }
                    _nodeBehavior = value;
                    UnityEditor.AssetDatabase.AddObjectToAsset(_nodeBehavior, this);
                }
                else
                {
                    _nodeBehavior = value;
                }
            }
        }

        protected Behavior.StatePhase _previousPhase;

        public override bool Process(BehaviorTree tree)
        {
            if(_nodeBehavior == null)
            {
                Debug.LogWarning(name + " is leaf node with unassigned nodeBehavior");
                return true;
            }

            Behavior.StatePhase phaseOnStateProcessing = _nodeBehavior.CurrentPhase;

            switch (_nodeBehavior.CurrentPhase)
            {
                case Behavior.StatePhase.ENTERING:
                    {
                        if (_previousPhase != _nodeBehavior.CurrentPhase)
                        {
                            _nodeBehavior.OnEnter(tree.currentBlackboard);
                        }
                        else
                        {
                            _nodeBehavior.EnterBehavior(tree.currentBlackboard);
                        }
                        break;
                    }
                case Behavior.StatePhase.ACTIVE:
                    {
                        _nodeBehavior.ActiveBehavior(tree.currentBlackboard);
                        break;
                    }
                case Behavior.StatePhase.EXITING:
                    {
                        if (_previousPhase != _nodeBehavior.CurrentPhase)
                        {
                            _nodeBehavior.OnExit(tree.currentBlackboard);
                        }
                        else
                        {
                            _nodeBehavior.ExitBehavior(tree.currentBlackboard);
                        }
                        break;
                    }
                case Behavior.StatePhase.INACTIVE:
                    {
                        return true;
                    }
            }

            return false;
        }

        public virtual void ForceBehaviorToEnd()
        {
            if(_nodeBehavior != null)
            {
                _nodeBehavior.ForceEndState();
            }
        }
    }
}