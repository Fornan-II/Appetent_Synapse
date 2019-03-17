using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace BehaviourTreeUI
{
    public class GraphSelector : BehaviorNode
    {
        public AI.Selector sourceNode;
        
        public string PropertyOneToEvaluate;
        public string PropertyTwoToEvaluate;
        public PropertyType PropertyTwoType = PropertyType.BOOL;
        public SelectorLogic.ComparisonMode Mode = SelectorLogic.ComparisonMode.EQUAL;

        public enum PropertyType
        {
            BOOL,
            INT,
            FLOAT,
            BLACKBOARD
        }

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

            foreach (NodeInfo bn in nextNodes)
            {
                if(bn.inSlot.name == "true:")
                {
                    sourceNode.nodeOnTrue = bn.node.GetAINode();
                }
                if(bn.inSlot.name == "false:")
                {
                    sourceNode.nodeOnFalse = bn.node.GetAINode();
                }
            }

            sourceNode.Logic.PropertyOneToEvaluate = PropertyOneToEvaluate;
            sourceNode.Logic.Mode = Mode;

            switch(PropertyTwoType)
            {
                case PropertyType.BLACKBOARD:
                    {
                        sourceNode.Logic.PropertyTwoToEvaluate = PropertyTwoToEvaluate;
                        break;
                    }
                case PropertyType.BOOL:
                    {
                        sourceNode.Logic.PropertyTwoToEvaluate = "(bool)" + PropertyTwoToEvaluate;
                        break;
                    }
                case PropertyType.FLOAT:
                    {
                        sourceNode.Logic.PropertyTwoToEvaluate = "(float)" + PropertyTwoToEvaluate;
                        break;
                    }
                case PropertyType.INT:
                    {
                        sourceNode.Logic.PropertyTwoToEvaluate = "(int)" + PropertyTwoToEvaluate;
                        break;
                    }
            }
        }
    }
}