using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Modifier))]
public class ModifierFloatPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float xPos = position.x;
        float boxWidth = (position.width - 10.0f) / 3.0f;

        Rect baseValueRect = new Rect(xPos, position.y, boxWidth, position.height);
        xPos += baseValueRect.width + 5;
        Rect modeRect = new Rect(xPos, position.y, boxWidth, position.height);
        xPos += modeRect.width + 5;
        Rect valueRect = new Rect(xPos, position.y, boxWidth, position.height);

        EditorGUI.PropertyField(baseValueRect, property.FindPropertyRelative("_baseValue"), GUIContent.none);
        EditorGUI.PropertyField(modeRect, property.FindPropertyRelative("ModifyMode"), GUIContent.none);
        GUI.enabled = false;
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("_value"), GUIContent.none);
        GUI.enabled = true;

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
