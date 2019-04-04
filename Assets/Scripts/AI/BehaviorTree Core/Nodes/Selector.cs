using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Selector 1", menuName = "BehaviorTree/Nodes/New Selector")]
    public class Selector : Node
    {
        public Node nodeOnTrue;
        public Node nodeOnFalse;
        public SelectorLogic Logic;

        public override bool Process(BehaviorTree tree)
        {
            bool value = false;
            if(Logic != null)
            {
                value = Logic.Evaluate(tree.currentAI.localBlackboard);
            }

            if(value)
            {
                return nodeOnTrue.Process(tree);
            }
            else
            {
                return nodeOnFalse.Process(tree);
            }
        }
    }
}