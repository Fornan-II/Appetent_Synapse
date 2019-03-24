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

        public override bool IsValid()
        {
            if (!sourceNode)
            {
                Debug.LogWarning(name + " somehow exists without an existing sourceNode!");
                return Validation(false);
            }

            return Validation(AllSlotsUsed());
        }

        protected override void SaveDataToAINode(AI.BehaviorTree tree, SaveTreeGraphNodeAsset nodeAssetSaver)
        {
            if(!IsValid()) { return; }

            List<NodeInfo> nextNodes = GetNextNodes();
            sourceNode.NextNode = nextNodes[0].node.GetAINode();

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(sourceNode);
        }

        public override void SaveDataRecursive(BehaviorTree tree, SaveTreeGraphNodeAsset nodeAssetSaver)
        {
            foreach(NodeInfo nextNode in GetNextNodes())
            {
                nextNode.node.SaveDataRecursive(tree, nodeAssetSaver);
            }

            SaveDataToAINode(tree, nodeAssetSaver);
        }
    }
}