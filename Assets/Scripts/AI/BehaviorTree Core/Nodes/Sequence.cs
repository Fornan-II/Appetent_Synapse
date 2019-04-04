using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Sequence 1", menuName = "BehaviorTree/Nodes/New Sequence")]
    public class Sequence : Node
    {
        public Node[] sequenceNodes;

        public override bool Process(BehaviorTree tree)
        {
            if (!tree.currentAI.instanceSequencePositions.ContainsKey(this))
            {
                tree.currentAI.instanceSequencePositions.Add(this, 0);
                tree.QueueNode(this);
            }

            if (tree.currentAI.instanceSequencePositions[this] >= sequenceNodes.Length)
            {
                tree.currentAI.instanceSequencePositions[this] = 0;
                return true;
            }

            Node nodeInSequence = sequenceNodes[tree.currentAI.instanceSequencePositions[this]];
            tree.currentAI.instanceSequencePositions[this]++;
            return nodeInSequence.Process(tree);
        }
    }
}