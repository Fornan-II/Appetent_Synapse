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

        public override bool IsValid()
        {
            if (!sourceNode)
            {
                return Validation(false);
            }

            return Validation(GetNextNodes().Count >=);
        }
    }
}