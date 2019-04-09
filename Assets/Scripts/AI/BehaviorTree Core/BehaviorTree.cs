using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI
{
    [CreateAssetMenu(fileName = "New Behavior Tree", menuName = "BehaviorTree/New Tree")]
    public class BehaviorTree : ScriptableObject
    {
        public Root root;

        [HideInInspector] public AIController currentAI;

        public virtual void ProcessTree(AIController ai)
        {
            currentAI = ai;
            Node activeNode = ai.ActiveNode;
            if(activeNode == null)
            {
                activeNode = root;
            }

            if(activeNode.Process(this))
            {
                if(currentAI.NodesToProcess.Count > 0)
                {
                    if(currentAI.ActiveNode is Sequence)
                    {
                        currentAI.instanceSequencePositions.Remove(currentAI.ActiveNode as Sequence);
                    }
                    //Debug.Log("Popping " + currentAI.ActiveNode + " for " + currentAI);
                    currentAI.NodesToProcess.Pop();
                }
            }
            currentAI = null;
        }

        public virtual void QueueNode(Node n)
        {
            if(currentAI == null)
            {
                //Debug.LogWarning("Could not QueueNode at this time - no AIController");
                return;
            }

            //Debug.Log("Queueing node " + n + " for " + currentAI);
            currentAI.NodesToProcess.Push(n);
        }

#if UNITY_EDITOR
        [ContextMenu("New Root")]
        public void CreateRoot()
        {
            //root = ScriptableObject.CreateInstance<Root>();
            root = CreateNode(ScriptableObject.CreateInstance<Root>()) as Root;
        }

        public Node CreateNode(Node newNode)
        {
            string treePath = GetNodeDirectory();
            string nodePath = treePath + "/" + name + "Nodes";
            if (!AssetDatabase.IsValidFolder(nodePath))
            {
                AssetDatabase.CreateFolder(treePath, name + "Nodes");
            }

            string fileName = newNode.GetType().ToString().Remove(0, 3) + " 1.asset";
            int assetNum = 2;
            while (AssetDatabase.IsMainAssetAtPathLoaded(nodePath + "/" + fileName))
            {
                fileName = fileName.Replace(" " + (assetNum - 1), " " + assetNum);
                assetNum++;
            }

            AssetDatabase.CreateAsset(newNode, nodePath + "/" + fileName);
            AssetDatabase.SaveAssets();

            return newNode;
        }

        public string GetNodeDirectory()
        {
            string treePath = AssetDatabase.GetAssetPath(GetInstanceID());
            return treePath.Remove(treePath.Length - (7 + name.Length));
        }
#endif
    }
}
