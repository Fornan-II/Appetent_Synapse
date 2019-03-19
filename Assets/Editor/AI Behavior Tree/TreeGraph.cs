﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public class TreeGraph : Graph
    {
        public AI.BehaviorTree Tree;
        protected BehaviorNode _rootNode;
        public string graphNodeFolderPath;
        protected bool _treeIsValid = false;

        public virtual void CreateTree()
        {
            if(!Tree)
            {
                Debug.LogWarning("Tree graph attempting to be created without a behavior tree object assigned");
                return;
            }

            CreateNode(Tree.root, Vector2.zero);
        }

        protected virtual Slot CreateNode(AI.Node n, Vector2 pos)
        {
            if(n is AI.Root)
            {
                AI.Root r = n as AI.Root;
                GraphRoot dispNode = BehaviorNode.NewRoot().node as GraphRoot;
                SaveGraphNodeAsset(dispNode, n.name);
                dispNode.sourceNode = r;
                dispNode.position.position = pos;

                if(r.NextNode)
                {
                    Slot nextInputSlot = CreateNode(r.NextNode, pos + new Vector2(150, 0));

                    Connect(dispNode.slots[0], nextInputSlot);
                }

                if(_rootNode)
                {
                    Debug.LogWarning("Tree Graph seems to already have root node? Overwriting.");
                }
                _rootNode = dispNode;

                AddNode(dispNode);
                return null;
            }
            else if(n is AI.Selector)
            {
                AI.Selector sel = n as AI.Selector;
                NodeInfo nodeInfo = BehaviorNode.NewSelector();
                GraphSelector dispNode = nodeInfo.node as GraphSelector;
                SaveGraphNodeAsset(dispNode, n.name);
                dispNode.sourceNode = sel;
                dispNode.position.position = pos;

                Slot o = dispNode.AddOutputSlot("true:");
                if (sel.nodeOnTrue)
                {
                    Slot nextInputSlot = CreateNode(sel.nodeOnTrue, pos + new Vector2(150, -50));
                    Connect(o, nextInputSlot);
                }

                o = dispNode.AddOutputSlot("false:");
                if (sel.nodeOnFalse)
                {
                    Slot nextInputSlot = CreateNode(sel.nodeOnFalse, pos + new Vector2(150, 50));
                    Connect(o, nextInputSlot);
                }

                AddNode(dispNode);
                return nodeInfo.inSlot;
            }
            else if(n is AI.Sequence)
            {
                AI.Sequence seq = n as AI.Sequence;
                NodeInfo nodeInfo = BehaviorNode.NewSequence();
                GraphSequence dispNode = nodeInfo.node as GraphSequence;
                SaveGraphNodeAsset(dispNode, n.name);
                dispNode.sourceNode = seq;
                dispNode.position.position = pos;

                int outName = 0;
                if(seq.sequenceNodes != null)
                {
                    float verticalOffset = 50.0f - ((seq.sequenceNodes.Length * 100.0f) / 2.0f);
                    foreach (AI.Node child in seq.sequenceNodes)
                    {
                        Slot o = dispNode.AddOutputSlot("out:" + outName);
                        Slot nextInputSlot = CreateNode(child, pos + new Vector2(150, verticalOffset));

                        Connect(o, nextInputSlot);

                        outName++;
                        verticalOffset += 100.0f;
                    }
                }

                AddNode(dispNode);
                return nodeInfo.inSlot;
            }
            else if(n is AI.Leaf)
            {
                AI.Leaf l = n as AI.Leaf;
                NodeInfo nodeInfo = BehaviorNode.NewLeaf();
                GraphLeaf dispNode = nodeInfo.node as GraphLeaf;
                SaveGraphNodeAsset(dispNode, n.name);
                dispNode.sourceNode = l;
                //dispNode.node.SetPropertyValue("Behavior Phase", l.nodeBehavior.CurrentPhase);
                dispNode.position.position = pos;

                AddNode(dispNode);
                return nodeInfo.inSlot;
            }

            Debug.LogWarning("Something went wrong in TreeGraph.CreateNode()");
            return null;
        }

        protected virtual void SaveGraphNodeAsset(Node n, string assetName)
        {
            string fileName = assetName + ".asset";

            AssetDatabase.CreateAsset(n, graphNodeFolderPath + "/" + fileName);
            AssetDatabase.SaveAssets();
        }

        public virtual void Validate()
        {
            _treeIsValid = true;
            if(!_rootNode)
            {
                _treeIsValid = false;
                return;
            }

            //Validating one node validates all the node's children.
            //_treeIsValid = _rootNode.IsValid(true);

            foreach(BehaviorNode bn in nodes)
            {
                if(!bn.IsValid())
                {
                    _treeIsValid = false;
                }
            }

            if(EditorApplication.isPlaying && _treeIsValid)
            {
                BehaviorNode activeNode = FindActiveNode(_rootNode);
                if(activeNode)
                {
                    activeNode.color = Styles.Color.Green;
                }
            }
        }

        protected virtual BehaviorNode FindActiveNode(BehaviorNode DEPRECATEDbn)
        {
            foreach(BehaviorNode bn in nodes)
            {
                if (bn.GetAINode() == Tree.ActiveNode)
                {
                    return bn;
                }
            }
            return null;

            //if (bn.sourceNode == Tree.ActiveNode)
            //{
            //    return bn;
            //}

            //foreach(Edge e in bn.outputEdges)
            //{
            //    if(e.toSlot != null)
            //    {
            //        if(e.toSlot.node)
            //        {
            //            if(e.toSlot.node is BehaviorNode)
            //            {
            //                return FindActiveNode(e.toSlot.node as BehaviorNode);
            //            }
            //        }
            //    }
            //}

            //return null;
        }

        #region Toolbar functionality
        public virtual void SaveGraphToSources()
        {
            if (!_treeIsValid)
            {
                return;
            }

            foreach (BehaviorNode bn in nodes)
            {
                bn.SaveDataToAINode(Tree);
            }
        }

        public virtual void CreateNewNode<T>(Vector2 pos) where T : AI.Node
        {
            AI.Node newNode = Tree.CreateNode(ScriptableObject.CreateInstance<T>());
            CreateNode(newNode, pos);
        }
        #endregion

        public override void DestroyNode(Node node)
        {
            string graphNodePath = AssetDatabase.GetAssetPath(node);

            if(node is BehaviorNode)
            {
                BehaviorNode bn = node as BehaviorNode;
                AI.Node ai = bn.GetAINode();
                if (ai)
                {
                    string AINodePath = AssetDatabase.GetAssetPath(ai);
                    AssetDatabase.DeleteAsset(AINodePath);
                }
                
            }

            base.DestroyNode(node);
            AssetDatabase.DeleteAsset(graphNodePath);
        }
    }
}