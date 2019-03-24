using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public delegate void SaveTreeGraphNodeAsset(Node n, string fileName);

    public struct NodeInfo
    {
        public BehaviorNode node;
        public Slot inSlot;

        public NodeInfo(BehaviorNode n, Slot s)
        {
            node = n;
            inSlot = s;
        }
    }

    public abstract class BehaviorNode : Node
    {
        #region Static Node Primatives
        public static NodeInfo NewRoot()
        {
            GraphRoot root = ScriptableObject.CreateInstance<GraphRoot>();

            root.Label = "Root";
            root.AddOutputSlot("out");

            return new NodeInfo(root, null);
        }

        public static NodeInfo NewSelector()
        {
            GraphSelector selector = ScriptableObject.CreateInstance<GraphSelector>();

            selector.Label = "Selector";
            Slot i = selector.AddInputSlot("in");
            //new AI.SelectorLogic().
            //Select number of slots based on AI.Selector output options

            return new NodeInfo(selector, i);
        }

        public static NodeInfo NewSequence()
        {
            GraphSequence sequence = ScriptableObject.CreateInstance<GraphSequence>();

            sequence.Label = "Sequence";
            Slot i = sequence.AddInputSlot("in");

            //Any number of outputs. This node stays active while waiting for it's outputs to execute.

            return new NodeInfo(sequence, i);
        }

        public static NodeInfo NewLeaf()
        {
            GraphLeaf leaf = ScriptableObject.CreateInstance<GraphLeaf>();

            leaf.Label = "Leaf";
            Slot i = leaf.AddInputSlot("in");

            //leaf.AddProperty(new Property(typeof(AI.Behavior), "Behavior"));

            return new NodeInfo(leaf, i);
        }
        #endregion

        public string Label;

        public abstract AI.Node GetAINode();

        protected abstract void SaveDataToAINode(AI.BehaviorTree tree, SaveTreeGraphNodeAsset nodeAssetSaver);

        public abstract void SaveDataRecursive(AI.BehaviorTree tree, SaveTreeGraphNodeAsset nodeAssetSaver);

        public abstract bool IsValid();

        protected virtual bool Validation(bool value)
        {
            if (value)
            {
                color = Styles.Color.Gray;
            }
            else
            {
                color = Styles.Color.Red;
            }

            return value;
        }

        public virtual List<NodeInfo> GetNextNodes()
        {
            List<NodeInfo> nextNodes = new List<NodeInfo>();

            foreach (Slot s in outputSlots)
            {
                if (s.edges.Count > 0)
                {
                    Node next = s.edges[0].toSlot.node;
                    if (next is BehaviorNode)
                    {
                        nextNodes.Add(new NodeInfo(next as BehaviorNode, s));
                    }
                    else
                    {
                        nextNodes.Add(new NodeInfo(null, s));
                    }
                }
                else
                {
                    nextNodes.Add(new NodeInfo(null, s));
                }
            }

            return nextNodes;
        }

        public virtual bool AllSlotsUsed()
        {
            foreach(Slot s in slots)
            {
                if(s.edges.Count <= 0)
                {
                    return false;
                }
                if(!s.edges[0].toSlot.node)
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual void OnValidate()
        {
            title = Label;
        }
    }
}