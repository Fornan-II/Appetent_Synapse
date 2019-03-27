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
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        Rect labelRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        letDisplay = EditorGUI.Foldout(position, letDisplay, label);
        position.y += labelRect.height + EditorGUIUtility.standardVerticalSpacing;
        if(letDisplay)
        {
            object val = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (val is Blackboard)
            {
                Blackboard bb = val as Blackboard;
                float yOffset = 0.0f;
                float yDelta = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                foreach (KeyValuePair<string, object> kvp in bb.Properties)
                {
                    Rect keyPos = new Rect(position.x, position.y + yOffset, 100, position.height);
                    Rect valPos = new Rect(position.x + 105, position.y + yOffset, 100, position.height);
                    string newKey = EditorGUI.TextField(keyPos, kvp.Key);
                    string newValue = EditorGUI.TextField(valPos, kvp.Value.ToString());

                    yOffset += yDelta;
                }

                Rect sillyRect = position;
                position.height = 1;
                position.y = yOffset;

                
                //EditorGUI.DrawRect(sillyRect, Color.red);
            }
        }
        // Set indent back to what it was

        EditorGUI.EndProperty();
    }
}
