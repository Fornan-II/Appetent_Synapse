using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(AI.BehaviorTree.Root))]
public class RootPropertyDrawer : PropertyDrawer
{
    private bool foldOut = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        foldOut = EditorGUI.Foldout(position, foldOut, label);
        if(foldOut)
        {
            position.y += position.height;
            SerializedProperty nextNode = property.FindPropertyRelative("NextNode");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("NextNode"));
            EditorGUI.PropertyField(position, nextNode);
        }
        
        EditorGUI.EndProperty();
    }

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    float baseHeight = base.GetPropertyHeight(property, label);
    //    if(foldOut)
    //    {
    //        SerializedProperty nextNode = property.FindPropertyRelative("NextNode");
    //        nextNode.
    //    }
    //}
}

[CustomPropertyDrawer(typeof(AI.BehaviorTree.Selector))]
public class SelectorPropertyDrawer : PropertyDrawer
{
    private bool foldOut = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        foldOut = EditorGUI.Foldout(position, foldOut, label);
        if (foldOut)
        {
            position.y += position.height;

            SerializedProperty nodeOnTrue = property.FindPropertyRelative("nodeOnTrue");
            SerializedProperty nodeOnFalse = property.FindPropertyRelative("nodeOnFalse");
            SerializedProperty selectorLogic = property.FindPropertyRelative("Logic");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("True Node"));
            EditorGUI.PropertyField(position, nodeOnTrue);
            position.y += EditorGUI.GetPropertyHeight(nodeOnTrue);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("False Node"));
            EditorGUI.PropertyField(position, nodeOnFalse);
            position.y += EditorGUI.GetPropertyHeight(nodeOnFalse);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Logic"));
            EditorGUI.PropertyField(position, selectorLogic);
        }

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(AI.BehaviorTree.Sequence))]
public class SequencePropertyDrawer : PropertyDrawer
{
    private bool foldOut = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        foldOut = EditorGUI.Foldout(position, foldOut, label);
        if (foldOut)
        {
            position.y += position.height;

            SerializedProperty nodeArr = property.FindPropertyRelative("sequenceNodes");
            for (int i = 0; i < nodeArr.arraySize; i++)
            {
                SerializedProperty element = nodeArr.GetArrayElementAtIndex(i);
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Node " + i));
                EditorGUI.PropertyField(position, element);
                position.y += EditorGUI.GetPropertyHeight(element);
            }
        }

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(AI.BehaviorTree.Leaf))]
public class LeafPropertyDrawer : PropertyDrawer
{
        private bool foldOut = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            foldOut = EditorGUI.Foldout(position, foldOut, label);
            if (foldOut)
            {
                position.y += position.height;
                SerializedProperty behavior = property.FindPropertyRelative("_behavior");
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Behavior"));
                EditorGUI.PropertyField(position, behavior);
            }

            EditorGUI.EndProperty();
        }
}