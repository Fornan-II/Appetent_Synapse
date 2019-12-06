using System.Collections.Generic;

namespace AI.BehaviorTree
{
    [System.Serializable]
    public class BehaviorTree : StateMachine.StateMachine
    {
        public Root root;

        //NodesToProcess used by BehaviorTree
        protected Stack<Sequence> SequencesToProcess = new Stack<Sequence>();
        //On the stack are any sequences that may be waiting to execute

        public override void Process(float deltaTime)
        {
            if(CurrentState == null)
            {
                if(SequencesToProcess.Count > 0)
                {
                    if(SequencesToProcess.Peek().SubsequentProcess(this))
                    {
                        SequencesToProcess.Pop();
                    }
                }
                else
                {
                    root.Process(this);
                }
            }

            base.Process(deltaTime);
        }

        public override void ForceEndState()
        {
            base.ForceEndState();
            SequencesToProcess.Clear();
        }

        public virtual void QueueNode(Node node)
        {
            if(node is Leaf)
            {
                CurrentState = (node as Leaf).Behavior;
                _currentStatePhase = StatePhase.ENTERING;
            }
            else if(node is Sequence)
            {
                SequencesToProcess.Push(node as Sequence);
            }
        }
    }
}
