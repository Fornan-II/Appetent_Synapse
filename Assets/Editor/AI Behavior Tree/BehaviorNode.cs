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

    public class BehaviorNode : Node
    {
        #region Static Node Primatives
        public static NodeInfo NewRoot()
        {
            BehaviorNode root = ScriptableObject.CreateInstance<BehaviorNode>();

            root.title = "Root";
            root.AddOutputSlot("out");
            root.position = new Rect(0, 0, 300, 200);

            return new NodeInfo(root, null);
        }

        public static NodeInfo NewSelector()
        {
            BehaviorNode selector = ScriptableObject.CreateInstance<BehaviorNode>();

            selector.title = "Selector";
            Slot i = selector.AddInputSlot("in");
            //root.position = new Rect(0, 0, 300, 200);

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

        protected bool Validation(bool value)
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

        public AI.Node sourceNode;

        public bool IsValid()
        {
            if (!sourceNode)
            {
                color = Styles.Color.Red;
                return Validation(false);
            }

            bool allOutputsValid = true;
            bool noOutputs = true;

            foreach (Slot s in outputSlots)
            {
                noOutputs = false;

                if (s.edges.Count > 1)
                {
                    allOutputsValid = false;
                }

                if (s.edges.Count >= 1)
                {
                    Node next = s.edges[0].toSlot.node;
                    if (next is BehaviorNode)
                    {
                        (next as BehaviorNode).IsValid();
                    }
                    else
                    {
                        allOutputsValid = false;
                    }
                }
                else
                {
                    allOutputsValid = false;
                }
            }
            
            if(noOutputs)
            {
                return Validation(sourceNode is AI.Leaf);
            }

            return Validation(allOutputsValid);
        }
    }
}