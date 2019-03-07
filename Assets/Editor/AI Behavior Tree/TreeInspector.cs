using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI.BehaviorTree))]
public class TreeInspector : Editor
{
    const string editorPath = "Assets/Editor/AI Behavior Tree";
    const string treeFolder = "TreeGraphs";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Open Graph Editor"))
        {
            if(target)
            {
                if(target is AI.BehaviorTree)
                {
                    BehaviourTreeUI.BehaviourTreeGraphEditorWindow.DoTree(GetTree(target as AI.BehaviorTree));
                }
            }
        }
    }

    protected BehaviourTreeUI.TreeGraph GetTree(AI.BehaviorTree sourceTree)
    {
        string treePath = editorPath + "/" + treeFolder;
        string fileName = sourceTree.name + "Graph.asset";

        if (AssetDatabase.IsMainAssetAtPathLoaded(treePath + "/" + fileName))
        {
            return AssetDatabase.LoadAssetAtPath<BehaviourTreeUI.TreeGraph>(treePath + "/" + fileName);
        }
        else
        {
            return GetNewTree(sourceTree);
        }
    }

    protected BehaviourTreeUI.TreeGraph GetNewTree(AI.BehaviorTree sourceTree)
    {
        //Initialize tree scriptable object
        BehaviourTreeUI.TreeGraph uiTree = ScriptableObject.CreateInstance<BehaviourTreeUI.TreeGraph>();

        //Save scriptable object
        //
        string treePath = editorPath + "/" + treeFolder;
        if (!AssetDatabase.IsValidFolder(treePath))
        {
            AssetDatabase.CreateFolder(editorPath, treeFolder);
        }

        string fileName = sourceTree.name + "Graph";

        AssetDatabase.CreateAsset(uiTree, treePath + "/" + fileName + ".asset");
        AssetDatabase.CreateFolder(treePath, fileName);
        AssetDatabase.SaveAssets();
        //

        //Generate tree
        //
        uiTree.Tree = target as AI.BehaviorTree;
        uiTree.nodeFolderPath = treePath + "/" + fileName;
        uiTree.CreateTree();
        //

        return uiTree;
    }
}
