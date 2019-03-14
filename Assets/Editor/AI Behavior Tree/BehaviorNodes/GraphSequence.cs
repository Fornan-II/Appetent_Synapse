using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeUI
{
    public class GraphSequence : BehaviorNode
    {
        public AI.Sequence sourceNode;

        public override AI.Node GetAINode()
        {
            return sourceNode;
        }

        public override bool IsValid(bool recursive = false)
        {
            if (!sourceNode)
            {
                return Validation(false);
            }

            List<BehaviorNode> nextNodes = GetNextNodes();
            bool validChildren = true;
            if (recursive)
            {
                foreach (BehaviorNode bn in nextNodes)
                {
                    if(bn)
                    {
                        if (!bn.IsValid(true))
                        {
                            validChildren = false;
                        }
                    }
                }
            }
            return Validation(validChildren && nextNodes.Count > 0 && !nextNodes.Contains(null));
        }

        public override void SaveDataToAINode()
        {
            if (!IsValid()) { return; }

            List<BehaviorNode> nextNodes = GetNextNodes();
            List<AI.Node> nextAINodes = new List<AI.Node>();

            foreach(BehaviorNode bn in nextNodes)
            {
                nextAINodes.Add(bn.GetAINode());
            }

            sourceNode.sequenceNodes = nextAINodes.ToArray();
        }
    }
}