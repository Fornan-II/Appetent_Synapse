using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI.BehaviorTree))]
public class TreeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Open Graph Editor"))
        {
            if(target)
            {
                if(target is AI.BehaviorTree)
                {
                    BehaviourTreeUI.BehaviourTreeGraphEditorWindow.DoTree(target as AI.BehaviorTree);
                }
            }
        }
    }
}
