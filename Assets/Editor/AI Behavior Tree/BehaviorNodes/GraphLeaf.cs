using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace BehaviourTreeUI
{
    public class GraphLeaf : BehaviorNode
    {
        public AI.Leaf sourceNode;

        public override Node GetAINode()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsValid(bool recursive = false)
        {
            if (!sourceNode)
            {
                return Validation(false);
            }
            //Also will need to check:
            //If a a behavior is chosen (maybe null behaviors are fine? Probably not);
            //If behavior is valid (may not be anything to check here)
            return Validation(true);
        }

        public override void SaveDataToAINode()
        {
            if (!IsValid()) { return; }

            //Set behavior node
        }
    }
}