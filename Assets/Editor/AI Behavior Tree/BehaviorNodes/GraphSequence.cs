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

            List<NodeInfo> nextNodes = GetNextNodes();
            bool validChildren = true;
            bool noNullValues = true;
            if (recursive)
            {
                foreach (NodeInfo bn in nextNodes)
                {
                    if (bn.node)
                    {
                        if (!bn.node.IsValid(true))
                        {
                            validChildren = false;
                        }
                    }
                    else
                    {
                        noNullValues = false;
                    }
                }
            }
            else
            {
                foreach (NodeInfo bn in nextNodes)
                {
                    if (!bn.node)
                    {
                        noNullValues = false;
                    }
                }
            }

            return Validation(validChildren && noNullValues);
        }

        public override void SaveDataToAINode()
        {
            if (!IsValid()) { return; }

            List<NodeInfo> nextNodes = GetNextNodes();
            List<AI.Node> nextAINodes = new List<AI.Node>();

            foreach(NodeInfo bn in nextNodes)
            {
                nextAINodes.Add(bn.node.GetAINode());
            }

            sourceNode.sequenceNodes = nextAINodes.ToArray();
        }
    }
}