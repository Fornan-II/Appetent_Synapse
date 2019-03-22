using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTreeUI
{
    [CustomEditor(typeof(GraphLeaf))]
    public class GraphLeafInspector : Editor
    {
        private int _choiceIndex = 0;
        private Type[] types;
        private string[] listOptions;
        private bool _initialized = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Refresh behaviors") || !_initialized)
            {
                types = BehaviorTreeUtil.GetClasses();
                listOptions = new string[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    if(!_initialized)
                    {
                        if(target is GraphLeaf)
                        {
                            GraphLeaf leaf = target as GraphLeaf;
                            if(leaf.sourceNode.nodeBehavior != null)
                            {
                                if(types[i] == leaf.sourceNode.nodeBehavior.GetType())
                                {
                                    _choiceIndex = i;
                                }
                            }
                        }
                    }
                    listOptions[i] = types[i].ToString();
                }

                if (!_initialized)
                {
                    _initialized = true;
                }
            }

            if (listOptions != null)
            {
                if(listOptions.Length > 0)
                {
                    _choiceIndex = EditorGUILayout.Popup(_choiceIndex, listOptions);
                }
            }

            if(target is GraphLeaf && 0 <= _choiceIndex && _choiceIndex < listOptions.Length && types != null)
            {
                GraphLeaf leaf = target as GraphLeaf;

                if(leaf.sourceNode.nodeBehavior == null)
                {
                    SetBehaviorType(ref leaf.sourceNode.nodeBehavior, types[_choiceIndex]);
                }
                else if (leaf.sourceNode.nodeBehavior.GetType() != types[_choiceIndex])
                {
                    SetBehaviorType(ref leaf.sourceNode.nodeBehavior, types[_choiceIndex]);
                }
            }
        }

        protected virtual void SetBehaviorType(ref AI.Behavior nodeBehavior, Type T)
        {
            object newBehavior = Activator.CreateInstance(T);
            if (newBehavior is AI.Behavior)
            {
                nodeBehavior = newBehavior as AI.Behavior;
            }
            else
            {
                Debug.LogWarning("nodeBehavior can only be assigned objects inheriting from AI.Behavior, not " + T);
            }
        }
    }
}