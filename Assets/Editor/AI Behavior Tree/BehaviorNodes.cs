using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public static class BehaviorNodes
    {
        public static Node NewRoot()
        {
            Node root = ScriptableObject.CreateInstance<Node>();

            root.title = "Root";
            root.AddOutputSlot("out");

            return root;
        }

        public static Node NewSelector()
        {
            Node selector = ScriptableObject.CreateInstance<Node>();

            selector.title = "Selector";
            selector.AddInputSlot("in");

            //selector.AddProperty(new Property(typeof(AI.Selector), "Logic"));

            //Select number of slots based on AI.Selector output options
            selector.AddOutputSlot("a");
            selector.AddOutputSlot("b");

            return selector;
        }

        public static Node NewSequence()
        {
            Node sequence = ScriptableObject.CreateInstance<Node>();

            sequence.title = "Sequence";
            sequence.AddInputSlot("in");

            //Any number of outputs. This node stays active while waiting for it's outputs to execute.
            sequence.AddOutputSlot("1");

            return sequence;
        }

        public static Node NewLeaf()
        {
            Node leaf = ScriptableObject.CreateInstance<Node>();

            leaf.title = "Leaf";
            leaf.AddInputSlot("in");

            //leaf.AddProperty(new Property(typeof(AI.Behavior), "Behavior"));

            return leaf;
        }
    }
}