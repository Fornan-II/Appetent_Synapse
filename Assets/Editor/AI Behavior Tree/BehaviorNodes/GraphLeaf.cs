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
            return sourceNode;
        }

        public override bool IsValid()
        {
            //Also will need to check:
            //If a a behavior is chosen (maybe null behaviors are fine? Probably not);
            //If behavior is valid (may not be anything to check here)
            return Validation(AllSlotsUsed());
        }

        protected override void SaveDataToAINode(AI.BehaviorTree tree, SaveTreeGraphNodeAsset nodeAssetSaver)
        {
            if (!IsValid()) { return; }

            if (!UnityEditor.AssetDatabase.Contains(sourceNode))
            {
                AI.Node aiNode = tree.CreateNode(sourceNode);
                nodeAssetSaver.Invoke(this, aiNode.name);
            }

            //Confirms that the nodeBehavior is saved to AssetDatabase
            sourceNode.NodeBehavior = sourceNode.NodeBehavior;

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(sourceNode);
        }

        public override void SaveDataRecursive(BehaviorTree tree, SaveTreeGraphNodeAsset nodeAssetSaver)
        {
            SaveDataToAINode(tree, nodeAssetSaver);
        }
    }
}