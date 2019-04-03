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

        [HideInInspector] public Blackboard currentBlackboard;

        public virtual void ProcessTree(Blackboard b)
        {
            currentBlackboard = b;
            Node activeNode = b.ActiveNode;
            if(activeNode == null)
            {
                activeNode = root;
            }

            if(activeNode.Process(this))
            {
                if(b.NodesToProcess.Count > 0)
                {
                    b.NodesToProcess.Pop();
                }
            }
            currentBlackboard = null;
        }

        public virtual void QueueNode(Node n)
        {
            if(currentBlackboard == null)
            {
                Debug.LogWarning("Could not QueueNode at this time - no blackboard");
                return;
            }
            currentBlackboard.NodesToProcess.Push(n);
        }

        public virtual void InterruptBehavior()
        {
            if (currentBlackboard == null)
            {
                Debug.LogWarning("Could not InterruptBehavior at this time - no blackboard");
                return;
            }
            Node activeNode = currentBlackboard.NodesToProcess.Peek();
            if(activeNode is Leaf)
            {
                (activeNode as Leaf).ForceBehaviorToEnd();
            }
            else
            {
                currentBlackboard.NodesToProcess.Clear();
            }
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
