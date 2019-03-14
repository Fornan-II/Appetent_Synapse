using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace BehaviourTreeUI
{
    public class GraphSelector : BehaviorNode
    {
        public AI.Selector sourceNode;

        public override Node GetAINode()
        {
            return sourceNode;
        }

        public override bool IsValid(bool recursive = false)
        {
            if (!sourceNode)
            {
                return Validation(false);
            }

            //Also will need to check:
            //If a selector statement is chosen
            //If selector statement is valid
            List<BehaviorNode> nextNodes = GetNextNodes();
            bool validChildren = true;
            if (recursive)
            {
                foreach (BehaviorNode bn in nextNodes)
                {
                    if (bn)
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

            foreach (BehaviorNode bn in nextNodes)
            {
                nextAINodes.Add(bn.GetAINode());
            }

            sourceNode.NextNodes = nextAINodes.ToArray();

            //Also need to save the selector
        }
    }
}