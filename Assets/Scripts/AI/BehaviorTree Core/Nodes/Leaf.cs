using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Leaf 1", menuName = "BehaviorTree/Nodes/New Leaf")]
    public class Leaf : Node
    {
        public Behavior nodeBehavior;

        protected Behavior.StatePhase _previousPhase;

        public override bool Process(BehaviorTree tree)
        {
            if(nodeBehavior == null)
            {
                Debug.LogWarning(name + " is leaf node with unassigned nodeBehavior");
                return true;
            }

            Behavior.StatePhase phaseOnStateProcessing = nodeBehavior.CurrentPhase;

            switch (nodeBehavior.CurrentPhase)
            {
                case Behavior.StatePhase.ENTERING:
                    {
                        if (_previousPhase != nodeBehavior.CurrentPhase)
                        {
                            nodeBehavior.OnEnter(tree.currentBlackboard);
                        }
                        else
                        {
                            nodeBehavior.EnterBehavior(tree.currentBlackboard);
                        }
                        break;
                    }
                case Behavior.StatePhase.ACTIVE:
                    {
                        nodeBehavior.ActiveBehavior(tree.currentBlackboard);
                        break;
                    }
                case Behavior.StatePhase.EXITING:
                    {
                        if (_previousPhase != nodeBehavior.CurrentPhase)
                        {
                            nodeBehavior.OnExit(tree.currentBlackboard);
                        }
                        else
                        {
                            nodeBehavior.ExitBehavior(tree.currentBlackboard);
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
            if(nodeBehavior != null)
            {
                nodeBehavior.ForceEndState();
            }
        }
    }
}