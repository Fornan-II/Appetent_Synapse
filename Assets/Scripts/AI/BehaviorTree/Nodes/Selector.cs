namespace AI.BehaviorTree
{
    public class Selector : Node
    {
        public Node nodeOnTrue;
        public Node nodeOnFalse;
        public SelectorLogic Logic;

        public Selector(Node trueNode, Node falseNode, SelectorLogic logic)
        {
            nodeOnTrue = trueNode;
            nodeOnFalse = falseNode;
            Logic = logic;
        }

        public override void Process(BehaviorTree tree)
        {
            bool value = false;
            if(Logic != null)
            {
                value = Logic.Evaluate(tree.Blackboard);
            }

            if(value)
            {
                nodeOnTrue.Process(tree);
            }
            else
            {
                nodeOnFalse.Process(tree);
            }
        }
    }
}