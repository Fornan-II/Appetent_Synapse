using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public class TreeGraph : Graph
    {
        public AI.BehaviorTree Tree;
        public string nodeFolderPath;
        public float nodeWidth = 300;
        public float nodeHeight = 200;
        public Dictionary<Node, AI.Node> nodeReference;

        public virtual void CreateTree()
        {
            if(!Tree)
            {
                Debug.LogWarning("Tree graph attempting to be created without a behavior tree object assigned");
                return;
            }

            nodeReference = new Dictionary<Node, AI.Node>();
            CreateNode(Tree.root, Vector2.zero);
        }

        protected virtual Slot CreateNode(AI.Node n, Vector2 pos)
        {
            if(n is AI.Root)
            {
                AI.Root r = n as AI.Root;
                Node dispNode = BehaviorNodes.NewRoot().node;
                SaveNode(dispNode);
                dispNode.position.position = pos;

                if(r.NextNode)
                {
                    Slot nextInputSlot = CreateNode(r.NextNode, pos + new Vector2(150, 0));

                    Connect(dispNode.slots[0], nextInputSlot);
                }

                AddNode(dispNode);
                nodeReference.Add(dispNode, n);
                return null;
            }
            else if(n is AI.Selector)
            {
                AI.Selector sel = n as AI.Selector;
                NodeInfo dispNode = BehaviorNodes.NewSelector();
                SaveNode(dispNode.node);
                dispNode.node.position.position = pos;

                int outName = 0;
                float verticalOffset = 0.0f - ((sel.NextNodes.Length * 100.0f) / 2.0f);
                foreach(AI.Node child in sel.NextNodes)
                {
                    Slot o = dispNode.node.AddOutputSlot("out:" + outName);
                    Slot nextInputSlot = CreateNode(child, pos + new Vector2(150, verticalOffset));

                    Connect(o, nextInputSlot);

                    outName++;
                    verticalOffset += 100.0f;
                }

                AddNode(dispNode.node);
                nodeReference.Add(dispNode.node, n);
                return dispNode.inSlot;
            }
            else if(n is AI.Sequence)
            {
                AI.Sequence seq = n as AI.Sequence;
                NodeInfo dispNode = BehaviorNodes.NewSequence();
                SaveNode(dispNode.node);
                dispNode.node.SetPropertyValue("Sequence Position", seq.SequencePosition);
                dispNode.node.position.position = pos;

                int outName = 0;
                float verticalOffset = 0.0f - ((seq.sequenceNodes.Length * 100.0f) / 2.0f);
                foreach (AI.Node child in seq.sequenceNodes)
                {
                    Slot o = dispNode.node.AddOutputSlot("out:" + outName);
                    Slot nextInputSlot = CreateNode(child, pos + new Vector2(150, verticalOffset));

                    Connect(o, nextInputSlot);

                    outName++;
                    verticalOffset += 100.0f;
                }

                AddNode(dispNode.node);
                nodeReference.Add(dispNode.node, n);
                return dispNode.inSlot;
            }
            else if(n is AI.Leaf)
            {
                AI.Leaf l = n as AI.Leaf;
                NodeInfo dispNode = BehaviorNodes.NewLeaf();
                SaveNode(dispNode.node);
                //dispNode.node.SetPropertyValue("Behavior Phase", l.nodeBehavior.CurrentPhase);
                dispNode.node.position.position = pos;

                AddNode(dispNode.node);
                nodeReference.Add(dispNode.node, n);
                return dispNode.inSlot;
            }

            Debug.LogWarning("Something went wrong in TreeGraph.CreateNode()");
            return null;
        }

        protected virtual void OnValidate()
        {
            Debug.Log("Validating " + name);
        }

        protected virtual void SaveNode(Node n)
        {
            string fileName = n.title + " 1.asset";
            int assetNum = 2;
            while (AssetDatabase.IsMainAssetAtPathLoaded(nodeFolderPath + "/" + fileName))
            {
                fileName = fileName.Replace(" " + (assetNum - 1), " " + assetNum);
                assetNum++;
            }

            AssetDatabase.CreateAsset(n, nodeFolderPath + "/" + fileName);
            AssetDatabase.SaveAssets();
        }
    }
}