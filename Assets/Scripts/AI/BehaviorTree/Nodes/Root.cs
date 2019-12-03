using UnityEngine;

namespace AI.BehaviorTree
{
    public class Root : Node
    {
        [SerializeField] protected Node NextNode;
        
        public Root(Node next)
        {
            NextNode = next;
        }

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