using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Sequence 1", menuName = "BehaviorTree/Nodes/New Sequence")]
    public class Sequence : Node
    {
        public Node[] sequenceNodes;
        protected int _sequencePosition = 0;
        public int SequencePosition { get { return _sequencePosition; } }

        public override bool Process(BehaviorTree tree)
        {
            if(_sequencePosition >= sequenceNodes.Length)
            {
                _sequencePosition = 0;
                return true;
            }

            if(_sequencePosition == 0)
            {
                tree.QueueNode(this);
            }

            Node nodeInSequence = sequenceNodes[_sequencePosition];
            _sequencePosition++;
            return nodeInSequence.Process(tree);
        }
    }
}