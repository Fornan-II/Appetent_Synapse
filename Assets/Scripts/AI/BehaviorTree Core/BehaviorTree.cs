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
        protected Stack<Node> _nodesToProcess;
        //On the stack:
        //1 : ActiveLeaf
        //2 : Any Sequences
        //If nothing, return to root
        public Node ActiveNode
        {
            get
            {
                if (_nodesToProcess == null)
                {
                    return null;
                }
                else if(_nodesToProcess.Count > 0)
                {
                    return _nodesToProcess.Peek();
                }
                else
                {
                    return root;
                }
            }
        }

        public virtual void ProcessTree(Blackboard b)
        {
            currentBlackboard = b;
            Node activeNode = _nodesToProcess.Peek();
            if(activeNode == null)
            {
                activeNode = root;
            }

            if(activeNode.Process(this))
            {
                if(_nodesToProcess.Count > 0)
                {
                    _nodesToProcess.Pop();
                }
            }
            currentBlackboard = null;
        }

        public virtual void QueueNode(Node n)
        {
            _nodesToProcess.Push(n);
        }

        public virtual void InterruptBehavior()
        {
            Node activeNode = _nodesToProcess.Peek();
            if(activeNode is Leaf)
            {
                (activeNode as Leaf).ForceBehaviorToEnd();
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
