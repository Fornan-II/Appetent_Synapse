using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public struct NodeInfo
    {
        public Node node;
        public Slot inSlot;

        public NodeInfo(Node n, Slot s)
        {
            node = n;
            inSlot = s;
        }
    }

    public static class BehaviorNodes
    {
        #region Node Primatives
        public static NodeInfo NewRoot()
        {
            Node root = ScriptableObject.CreateInstance<Node>();

            root.title = "Root";
            root.AddOutputSlot("out");
            root.position = new Rect(0, 0, 300, 200);

            return new NodeInfo(root, null);
        }

        public static NodeInfo NewSelector()
        {
            Node selector = ScriptableObject.CreateInstance<Node>();

            selector.title = "Selector";
            Slot i = selector.AddInputSlot("in");
            //root.position = new Rect(0, 0, 300, 200);

            //selector.AddProperty(new Property(typeof(AI.Selector), "Logic"));

            //Select number of slots based on AI.Selector output options

            return new NodeInfo(selector, i);
        }

        public static NodeInfo NewSequence()
        {
            Node sequence = ScriptableObject.CreateInstance<Node>();

            sequence.title = "Sequence";
            Slot i = sequence.AddInputSlot("in");

            sequence.AddProperty(new Property(typeof(int), "Sequence Position"));

            //Any number of outputs. This node stays active while waiting for it's outputs to execute.

            return new NodeInfo(sequence, i);
        }

        public static NodeInfo NewLeaf()
        {
            Node leaf = ScriptableObject.CreateInstance<Node>();

            leaf.title = "Leaf";
            Slot i = leaf.AddInputSlot("in");

            leaf.AddProperty(new Property(typeof(AI.Behavior.StatePhase), "Behavior Phase"));

            //leaf.AddProperty(new Property(typeof(AI.Behavior), "Behavior"));

            return new NodeInfo(leaf, i);
        }
#endregion

        public static NodeInfo GetGraphNodeFromAINode(AI.Node source)
        {
            NodeInfo result;

            if (source is AI.Root)
            {
                result = NewRoot();
            }
            else if (source is AI.Selector)
            {
                AI.Selector sel = source as AI.Selector;
                result = NewSelector();
                //WILL COME BACK TO REVISE THIS LATER I GUESS
                for (int i = 0; i < sel.NextNodes.Length; i++)
                {
                    result.node.AddOutputSlot("out: " + i);
                }
            }
            else if(source is AI.Sequence)
            {
                AI.Sequence seq = source as AI.Sequence;
                result = NewSequence();
                
                result.node.SetPropertyValue("Sequence Position", seq.SequencePosition);

                for(int i = 0; i < seq.sequenceNodes.Length; i++)
                {
                    result.node.AddOutputSlot("out: " + i);
                }
            }
            else if(source is AI.Leaf)
            {
                AI.Leaf leaf = source as AI.Leaf;
                result = NewLeaf();

                result.node.SetPropertyValue("Behavior Phase", leaf.nodeBehavior.CurrentPhase);
            }
            else
            {
                result = new NodeInfo(ScriptableObject.CreateInstance<Node>(), null);
                result.node.title = "UNKNOWN TYPE";
            }

            return result;
        }

        public static void UpdateTreeFromGraph(AI.BehaviorTree result, Graph source)
        {
            foreach(Node n in source.nodes)
            {
                
            }
        }
    }
}