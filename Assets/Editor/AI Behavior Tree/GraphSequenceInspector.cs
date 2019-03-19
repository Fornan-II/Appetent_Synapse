using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTreeUI
{
    [CustomEditor(typeof(GraphSequence))]
    public class GraphSequenceInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add output"))
            {
                if (target)
                {
                    if (target is GraphSequence)
                    {
                        (target as GraphSequence).AddOutput();
                    }
                }
            }
            if (GUILayout.Button("Remove output"))
            {
                if (target)
                {
                    if (target is GraphSequence)
                    {
                        (target as GraphSequence).RemoveOutput();
                    }
                }
            }
        }
    }
}