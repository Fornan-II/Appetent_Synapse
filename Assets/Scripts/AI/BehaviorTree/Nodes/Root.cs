using UnityEngine;

namespace AI.BehaviorTree
{
    [System.Serializable]
    public class Root : Node
    {
        public Node NextNode;
        
        public Root() { }

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