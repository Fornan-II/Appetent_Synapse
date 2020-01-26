using AI.StateMachine;
using UnityEngine;

namespace AI.BehaviorTree
{
    [System.Serializable]
    public class Leaf : Node
    {
        public State Behavior;

        public Leaf() { }

        public Leaf(State behavior)
        {
            Behavior = behavior;
        }

        public override void Process(BehaviorTree tree)
        {
            tree.QueueNode(this);
        }
    }
}