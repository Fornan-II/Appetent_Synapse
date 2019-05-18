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

        Rect baseValueRect = new Rect(position.x, position.y, position.width / 2.0f - 2.5f, position.height);
        Rect valueRect = new Rect(position.x + baseValueRect.width + 5, position.y, position.width / 2.0f - 2.5f, position.height);

        SerializedProperty baseValue = property.FindPropertyRelative("_baseValue");
        EditorGUI.PropertyField(baseValueRect, baseValue, GUIContent.none);
        GUI.enabled = false;
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("_value"), GUIContent.none);
        GUI.enabled = true;

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
