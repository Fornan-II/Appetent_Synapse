using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTreeUI
{
    [CustomEditor(typeof(GraphLeaf))]
    public class GraphLeafInspector : Editor
    {
        int _choiceIndex = 0;
        string[] _choices;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(_choices == null)
            {
                _choices = new string[0];
            }

            if (GUILayout.Button("Refresh behaviors"))
            {
                _choices = AI.AIBehavior.GetClassNames();
            }

            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);

            EditorUtility.SetDirty(target);
        }
    }
}