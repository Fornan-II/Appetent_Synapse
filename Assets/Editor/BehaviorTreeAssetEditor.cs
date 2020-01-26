using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using AI;
using AI.BehaviorTree;

[CustomEditor(typeof(BehaviorTreeAsset))]
public class BehaviorTreeAssetEditor : Editor
{
    private enum NodeType
    {
        UNSELECTED,
        SELECTOR,
        SEQUENCE,
        LEAF
    }

    public override void OnInspectorGUI()
    {
        BehaviorTreeAsset targetTreeAsset = target as BehaviorTreeAsset;
        EditorGUILayout.LabelField("ROOT");
        DrawNodeGUI(targetTreeAsset.TreeRoot.NextNode);
        //base.OnInspectorGUI();
    }

    protected virtual void DrawNodeGUI(Node node, bool endHorizontal = false)
    {
        int startingIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel++;

        NodeType originalNodeType = GetNodeType(node);
        NodeType nodeType = (NodeType)EditorGUILayout.EnumPopup(originalNodeType);
        if (endHorizontal)
            EditorGUILayout.EndHorizontal();

        if (originalNodeType != nodeType)
        {
            //string msg = "Changing " + originalNodeType + " to " + nodeType + ".\n" + node.GetType() + " --> ";
            Node newNode = MakeNodeOfType(nodeType);
            ReplaceNode(node, newNode);
            node = newNode;
            //Debug.Log(msg + node.GetType());
        }

        if (nodeType == NodeType.SELECTOR)
        {
            Selector selector = node as Selector;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("True");
            DrawNodeGUI(selector.nodeOnTrue, true);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("False");
            DrawNodeGUI(selector.nodeOnFalse, true);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Logic");
            EditorGUILayout.LabelField(selector.Logic.ToString());
            //EditorGUILayout.PropertyField(serializedObject.)
            EditorGUILayout.EndHorizontal();
        }
        else if (nodeType == NodeType.SEQUENCE)
        {
            Sequence sequence = node as Sequence;

            for (int i = 0; i < sequence.sequenceNodes.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(i.ToString());
                DrawNodeGUI(sequence.sequenceNodes[i], true);
            }
        }
        else if (nodeType == NodeType.LEAF)
        {
            //EditorGUILayout.BeginHorizontal();
            Leaf leaf = node as Leaf;
            if(leaf.Behavior == null)
            {
                EditorGUILayout.LabelField("Unselected behavior");
            }
            else
            {
                EditorGUILayout.LabelField((node as Leaf).Behavior.Label);
            }
            //EditorGUILayout.EndHorizontal();
        }

        //EditorGUILayout.LabelField(node.GetJSONSerialized());

        EditorGUI.indentLevel = startingIndentLevel;
    }

    private NodeType GetNodeType(Node node)
    {
        if(node is Selector)
        {
            return NodeType.SELECTOR;
        }
        else if(node is Sequence)
        {
            return NodeType.SEQUENCE;
        }
        else if(node is Leaf)
        {
            return NodeType.LEAF;
        }

        return NodeType.UNSELECTED;
    }

    private Node MakeNodeOfType(NodeType nodeType)
    {
        if(nodeType == NodeType.SELECTOR)
        {
            return new Selector();
        }
        else if(nodeType == NodeType.SEQUENCE)
        {
            return new Sequence();
        }
        else if(nodeType == NodeType.LEAF)
        {
            return new Leaf();
        }

        return null;
    }

    private void ReplaceNode(Node original, Node replacement)
    {
        FindAndReplaceNode((target as BehaviorTreeAsset).TreeRoot, original, replacement);
    }

    private bool FindAndReplaceNode(Node currentSearchPosition, Node original, Node replacement)
    {
        if (currentSearchPosition is Root)
        {
            Root asRoot = currentSearchPosition as Root;
            if (asRoot.NextNode == original)
            {
                asRoot.NextNode = replacement;
                return true;
            }
            else
            {
                FindAndReplaceNode(asRoot.NextNode, original, replacement);
            }
        }
        else if(currentSearchPosition is Selector)
        {
            Selector asSelector = currentSearchPosition as Selector;
            if(asSelector.nodeOnTrue == original)
            {
                asSelector.nodeOnTrue = replacement;
                return true;
            }
            else if(asSelector.nodeOnFalse == original)
            {
                asSelector.nodeOnFalse = replacement;
                return true;
            }
            else
            {
                if(!FindAndReplaceNode(asSelector.nodeOnTrue, original, replacement))
                {
                    return FindAndReplaceNode(asSelector.nodeOnFalse, original, replacement);
                }
            }
        }
        else if(currentSearchPosition is Sequence)
        {
            Sequence asSequence = currentSearchPosition as Sequence;
            for(int i = 0; i < asSequence.sequenceNodes.Length; i++)
            {
                if(asSequence.sequenceNodes[i] == original)
                {
                    asSequence.sequenceNodes[i] = replacement;
                    return true;
                }
            }

            for(int i = 0; i < asSequence.sequenceNodes.Length; i++)
            {
                bool found = FindAndReplaceNode(asSequence.sequenceNodes[i], original, replacement);
                if(found)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
