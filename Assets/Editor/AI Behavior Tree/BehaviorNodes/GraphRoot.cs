using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;
using AI;

namespace BehaviourTreeUI
{
    public class GraphRoot : BehaviorNode
    {
        public AI.Root sourceNode;

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
                foreach(NodeInfo bn in nextNodes)
                {
                    if(!bn.node)
                    {
                        noNullValues = false;
                    }
                }
            }
            
            return Validation(validChildren && nextNodes.Count == 1 && noNullValues);
        }

        public override void SaveDataToAINode()
        {
            if(!IsValid()) { return; }

            List<NodeInfo> nextNodes = GetNextNodes();
            sourceNode.NextNode = nextNodes[0].node.GetAINode();
        }
    }
}