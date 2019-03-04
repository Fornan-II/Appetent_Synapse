﻿using System.Collections;
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
            Behavior.StatePhase phaseOnStateProcessing = nodeBehavior.CurrentPhase;

            switch (nodeBehavior.CurrentPhase)
            {
                case Behavior.StatePhase.ENTERING:
                    {
                        if (_previousPhase != nodeBehavior.CurrentPhase)
                        {
                            nodeBehavior.OnEnter(tree.blackboard);
                        }
                        else
                        {
                            nodeBehavior.EnterBehavior(tree.blackboard);
                        }
                        break;
                    }
                case Behavior.StatePhase.ACTIVE:
                    {
                        nodeBehavior.ActiveBehavior(tree.blackboard);
                        break;
                    }
                case Behavior.StatePhase.EXITING:
                    {
                        if (_previousPhase != nodeBehavior.CurrentPhase)
                        {
                            nodeBehavior.OnExit(tree.blackboard);
                        }
                        else
                        {
                            nodeBehavior.ExitBehavior(tree.blackboard);
                        }
                        break;
                    }
                case Behavior.StatePhase.INACTIVE:
                    {
                        nodeBehavior = null;
                        return true;
                    }
            }

            return false;
        }
    }
}