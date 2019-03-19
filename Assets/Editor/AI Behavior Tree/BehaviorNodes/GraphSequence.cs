using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public class GraphSequence : BehaviorNode
    {
        public AI.Sequence sourceNode;

        public override AI.Node GetAINode()
        {
            return sourceNode;
        }

        public override bool IsValid()
        {
            return Validation(AllSlotsUsed());
        }

        public override void SaveDataToAINode(AI.BehaviorTree tree)
        {
            if (!IsValid()) { return; }

            if(!sourceNode)
            {
                tree.CreateNode(ScriptableObject.CreateInstance<AI.Sequence>());
            }

            List<NodeInfo> nextNodes = GetNextNodes();
            List<AI.Node> nextAINodes = new List<AI.Node>();

            foreach(NodeInfo bn in nextNodes)
            {
                nextAINodes.Add(bn.node.GetAINode());
            }

            sourceNode.sequenceNodes = nextAINodes.ToArray();
        }

        public virtual void AddOutput()
        {
            int highestOutPutNum = -1;
            foreach (Slot s in outputSlots)
            {
                string slotNumAsString = s.name.Substring(4);
                int slotNum;
                if (int.TryParse(slotNumAsString, out slotNum))
                {
                    if (slotNum > highestOutPutNum)
                    {
                        highestOutPutNum = slotNum;
                    }
                }
            }

            AddOutputSlot("out: " + (highestOutPutNum + 1));
        }

        public virtual void RemoveOutput()
        {
            int highestOutPutNum = -1;
            Slot highestSlot = null;
            foreach (Slot s in outputSlots)
            {
                string slotNumAsString = s.name.Substring(4);
                int slotNum;
                if (int.TryParse(slotNumAsString, out slotNum))
                {
                    if (slotNum > highestOutPutNum)
                    {
                        highestOutPutNum = slotNum;
                        highestSlot = s;
                    }
                }
            }

            if (highestSlot != null)
            {
                RemoveSlot(highestSlot);
            }
        }
    }
}