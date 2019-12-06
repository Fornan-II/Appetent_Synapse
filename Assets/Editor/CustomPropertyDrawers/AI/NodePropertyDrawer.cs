using UnityEditor;
using UnityEngine;

namespace AI.Editor
{
    [CustomPropertyDrawer(typeof(BehaviorTree.Node))]
    public class NodePropertyDrawer : PropertyDrawer
    {
        private int selected = 0;

        private static string[] SubTypeNames = new string[] { "Root", "Selector", "Sequence", "Leaf" };
        private static PropertyDrawer[] SubTypeDrawers = new PropertyDrawer[]
        {
            //new RootPropertyDrawer(),
            //new SelectorPropertyDrawer(),
            //new SequencePropertyDrawer(),
            //new LeafPropertyDrawer()
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            System.Type t = property.serializedObject.targetObject.GetType();
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(t.ToString()));
            //EditorGUI.PropertyField(position, new UnityEditor.SerializedObject());
            selected = EditorGUI.Popup(position, 0, SubTypeNames);

            //SubTypeDrawers[selected].OnGUI(position, property, label);

            EditorGUI.EndProperty();
        }
    }
}