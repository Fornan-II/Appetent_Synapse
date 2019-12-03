using AI.StateMachine;
using UnityEngine;

namespace AI.BehaviorTree
{
    public class Leaf : Node
    {
        [SerializeField]protected State _behavior;
        public State Behavior { get { return _behavior; } }

        public Leaf(State behavior)
        {
            _behavior = behavior;
        }

        public override void Process(BehaviorTree tree)
        {
            tree.QueueNode(this);
        }
    }
}