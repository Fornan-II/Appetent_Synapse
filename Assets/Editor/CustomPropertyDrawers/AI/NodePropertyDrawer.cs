using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AI.Editor
{
    [CustomPropertyDrawer(typeof(BehaviorTree.Node))]
    public class NodePropertyDrawer : PropertyDrawer
    {
        //    private int selected = 0;

        //    //Idea of Dictionary: https://answers.unity.com/questions/1499762/using-custom-property-drawers-with-polymorphism.html
        //    private static string[] SubTypeNames = new string[] { "Unselected", "Selector", "Sequence", "Leaf" };
        //    private static Dictionary<Type, int> SubTypeIndeces = new Dictionary<Type, int>
        //    {
        //        { typeof(BehaviorTree.Selector), 1 },
        //        { typeof(BehaviorTree.Sequence), 2 },
        //        { typeof(BehaviorTree.Leaf), 3 }
        //    };

        //    private static PropertyDrawer[] SubTypeDrawers = new PropertyDrawer[]
        //    {
        //        null,
        //        new SelectorPropertyDrawer(),
        //        new SequencePropertyDrawer(),
        //        new LeafPropertyDrawer()
        //    };

        static Dictionary<Type, PropertyDrawer> SubTypeDrawers = new Dictionary<Type, PropertyDrawer>()
        {
            { typeof(BehaviorTree.Root), new RootPropertyDrawer() },
            { typeof(BehaviorTree.Selector), new SelectorPropertyDrawer() },
            { typeof(BehaviorTree.Sequence), new SequencePropertyDrawer() },
            { typeof(BehaviorTree.Leaf), new LeafPropertyDrawer() }
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            UnityEngine.Object obj = property.objectReferenceValue;
            if(obj)
            {
                SubTypeDrawers[obj.GetType()].OnGUI(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, "NULL");
            }

    //        if (property.serializedObject.targetObject == null)
    //        {
    //            selected = 0;
    //        }
    //        else
    //        {
    //            Type targetType = property.serializedObject.targetObject.GetType();
    //            if(SubTypeIndeces.ContainsKey(targetType))
    //            {
    //                selected = SubTypeIndeces[targetType];

    //            }
    //        }
    //        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Node"));
    //        //EditorGUI.PropertyField(position, new UnityEditor.SerializedObject());
    //        int oldSelected = selected;
    //        selected = EditorGUI.Popup(position, selected, SubTypeNames);

    //        //SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue as Monkey);

    //        position.y += position.height;
    //        //EditorGUI.LabelField(position, property.objectReferenceValue.ToString());

    //        if (oldSelected != selected)
    //        {
                
    //        }

    //        //SubTypeDrawers[selected].OnGUI(position, property, label);

            EditorGUI.EndProperty();
        }
    }
}