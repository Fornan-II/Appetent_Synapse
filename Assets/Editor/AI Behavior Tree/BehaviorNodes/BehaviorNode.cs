using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
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
            BehaviorNode root = ScriptableObject.CreateInstance<BehaviorNode>();

            root.title = "Root";
            root.AddOutputSlot("out");

            return new NodeInfo(root, null);
        }

        public static NodeInfo NewSelector()
        {
            BehaviorNode selector = ScriptableObject.CreateInstance<BehaviorNode>();

            selector.title = "Selector";
            Slot i = selector.AddInputSlot("in");

            //selector.AddProperty(new Property(typeof(AI.Selector), "Logic"));

            //Select number of slots based on AI.Selector output options

            return new NodeInfo(selector, i);
        }

        public static NodeInfo NewSequence()
        {
            BehaviorNode sequence = ScriptableObject.CreateInstance<BehaviorNode>();

            sequence.title = "Sequence";
            Slot i = sequence.AddInputSlot("in");

            sequence.AddProperty(new Property(typeof(int), "Sequence Position"));

            //Any number of outputs. This node stays active while waiting for it's outputs to execute.

            return new NodeInfo(sequence, i);
        }

        public static NodeInfo NewLeaf()
        {
            BehaviorNode leaf = ScriptableObject.CreateInstance<BehaviorNode>();

            leaf.title = "Leaf";
            Slot i = leaf.AddInputSlot("in");

            leaf.AddProperty(new Property(typeof(AI.Behavior.StatePhase), "Behavior Phase"));

            //leaf.AddProperty(new Property(typeof(AI.Behavior), "Behavior"));

            return new NodeInfo(leaf, i);
        }
        #endregion

        public abstract AI.Node GetAINode();

        public abstract void SaveDataToAINode();

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

        public virtual List<BehaviorNode> GetNextNodes()
        {
            List<BehaviorNode> nextNodes = new List<BehaviorNode>();

            foreach (Edge e in outputEdges)
            {
                Node next = e.toSlot.node;
                if(next is BehaviorNode)
                {
                    nextNodes.Add(next as BehaviorNode);
                }
            }

            return nextNodes;
        }
    }
}