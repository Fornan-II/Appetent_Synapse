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

        public override bool IsValid()
        {
            if (!sourceNode)
            {
                return Validation(false);
            }

            return Validation(GetNextNodes().Count == 1);
        }

        public override void SaveDataToAINode()
        {
            throw new System.NotImplementedException();
        }
    }
}