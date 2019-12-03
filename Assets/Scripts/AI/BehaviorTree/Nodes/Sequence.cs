namespace AI.BehaviorTree
{
    public class Sequence : Node
    {
        public Node[] sequenceNodes;
        protected int _sequencePosition;

        public Sequence(Node[] nodes)
        {
            sequenceNodes = nodes;
            _sequencePosition = 0;
        }

        public override void Process(BehaviorTree tree)
        {
            _sequencePosition = 0;
            if(SubsequentProcess(tree))
            {
                tree.QueueNode(this);
            }
        }

        public virtual bool SubsequentProcess(BehaviorTree tree)
        {
            if (_sequencePosition >= sequenceNodes.Length)
            {
                return true;
            }

            sequenceNodes[_sequencePosition].Process(tree);
            ++_sequencePosition;
            return false;
        }
    }
}