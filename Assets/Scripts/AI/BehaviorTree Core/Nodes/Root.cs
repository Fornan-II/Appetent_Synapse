using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Root 1", menuName = "BehaviorTree/Nodes/New Root")]
    public class Root : Node
    {
        public Node NextNode;
        
        public override bool Process(BehaviorTree tree)
        {
            if(NextNode == null)
            {
                Debug.LogWarning("Root node finished processing as non-leaf node");
                return true;
            }
            
            return NextNode.Process(tree);
        }
    }
}