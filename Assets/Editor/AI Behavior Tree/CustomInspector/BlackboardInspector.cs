using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AI;

[CustomPropertyDrawer(typeof(Blackboard))]
public class BlackboardInspector : PropertyDrawer
{
    protected bool letDisplay = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty bbProperties = property.FindPropertyRelative("Properties");
        SerializedProperty keys = bbProperties.FindPropertyRelative("keys");
        SerializedProperty valueTypes = bbProperties.FindPropertyRelative("valueTypes");
        SerializedProperty serializedValues = bbProperties.FindPropertyRelative("serializedValues");

        Rect propertyRect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight, position.width / 3.0f, EditorGUIUtility.singleLineHeight);

        //EditorGUI.LabelField(position, "Blackboard \"" + property.name + "\" Properties");
        letDisplay = EditorGUI.Foldout(position, letDisplay, "Blackboard \"" + property.name + "\" Properties");
        if (letDisplay)
        {
            int max = keys.arraySize;
            EditorGUI.indentLevel++;
            if(max <= 0)
            {
                EditorGUI.LabelField(propertyRect, "No properties currently.");
            }
            for (int i = 0; i < max; i++)
            {
                float startX = propertyRect.x;
                EditorGUI.PropertyField(propertyRect, keys.GetArrayElementAtIndex(i), GUIContent.none);
                propertyRect.x += propertyRect.width;
                EditorGUI.PropertyField(propertyRect, valueTypes.GetArrayElementAtIndex(i), GUIContent.none);
                propertyRect.x += propertyRect.width;
                EditorGUI.PropertyField(propertyRect, serializedValues.GetArrayElementAtIndex(i), GUIContent.none);
                propertyRect.x = startX;

                propertyRect.y += propertyRect.height + EditorGUIUtility.standardVerticalSpacing;
            }

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(property.type == "Blackboard")
        {
            SerializedProperty bbProperties = property.FindPropertyRelative("Properties");
            SerializedProperty keys = bbProperties.FindPropertyRelative("keys");
            if (letDisplay)
            {
                int keyCount = keys.arraySize;
                if(keyCount <= 0)
                {
                    keyCount = 1;
                }
                return (keyCount + 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        else
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}