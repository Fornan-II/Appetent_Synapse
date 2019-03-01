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

            selector.title = "New Selector";
            selector.AddInputSlot("in");

            //selector.AddProperty(new Property(typeof(AI.Selector), "Logic"));

            //Select number of slots based on AI.Selector output options
            selector.AddOutputSlot("a");
            selector.AddOutputSlot("b");

            return selector;
        }

        public static Node Sequence NewSequence()
        {
            Node sequence = ScriptableObject.CreateInstance<Node>();

            sequence.title = "New Sequence";
            sequence.AddInputSlot("in");

            sequence.AddOutputSlot("out");
        }
    }
}