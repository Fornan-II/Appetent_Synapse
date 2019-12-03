using UnityEngine;

namespace AI.BehaviorTree
{
    public class Root : Node
    {
        protected Node NextNode;
        
        public override void Process(BehaviorTree tree)
        {
            if (NextNode == null)
            {
                Debug.LogWarning("Root node finished processing as non-leaf node");
            }
            else
            {
                NextNode.Process(tree);
            }
        }
    }
}