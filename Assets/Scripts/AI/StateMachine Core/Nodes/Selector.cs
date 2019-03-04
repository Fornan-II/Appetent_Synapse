using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Selector 1", menuName = "BehaviorTree/Nodes/New Selector")]
    public class Selector : Node
    {
        public Node[] NextNodes;

        protected virtual int NodeLogic()
        {
            //Returns index of NextNodes to use
            return 0;
        }

        public override bool Process(BehaviorTree tree)
        {
            int nodeIndex = NodeLogic();
            if(0 <= nodeIndex && nodeIndex > NextNodes.Length)
            {
                return NextNodes[NodeLogic()].Process(tree);
            }

            Debug.LogWarning("Selector node finished processing as non-leaf node");
            return true;
        }
    }
}