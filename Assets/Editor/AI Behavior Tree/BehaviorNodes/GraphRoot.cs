using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

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

            List<BehaviorNode> nextNodes = GetNextNodes();
            bool validChildren = true;
            if(recursive)
            {
                foreach(BehaviorNode bn in nextNodes)
                {
                    if(bn)
                    {
                        if(!bn.IsValid(true))
                        {
                            validChildren = false;
                        }
                    }
                }
            }
            return Validation(validChildren && nextNodes.Count == 1 && !nextNodes.Contains(null));
        }

        public override void SaveDataToAINode()
        {
            if(!IsValid()) { return; }

            List<BehaviorNode> nextNodes = GetNextNodes();
            sourceNode.NextNode = nextNodes[0].GetAINode();
        }
    }
}