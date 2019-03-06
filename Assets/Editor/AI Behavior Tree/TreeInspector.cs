using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI.BehaviorTree))]
public class TreeInspector : Editor
{
    protected BehaviourTreeUI.TreeGraph treeRepresentation;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Open Graph Editor"))
        {
            if(target)
            {
                if(target is AI.BehaviorTree)
                {
                    if(treeRepresentation == null)
                    {
                        treeRepresentation = GetNewTree(target as AI.BehaviorTree);
                    }
                    BehaviourTreeUI.BehaviourTreeGraphEditorWindow.DoTree(treeRepresentation);
                }
            }
        }
    }

    protected BehaviourTreeUI.TreeGraph GetTree(AI.BehaviorTree sourceTree)
    {
        string editorPath = "Assets/Editor";
        string treeFolder = "TreeGraphs";
        string treePath = editorPath + "/" + treeFolder;

        //if (AssetDatabase.IsMainAssetAtPathLoaded(treePath + "/" + fileName))
        //{

        //}

        return null;
    }

    protected BehaviourTreeUI.TreeGraph GetNewTree(AI.BehaviorTree sourceTree)
    {
        BehaviourTreeUI.TreeGraph uiTree = ScriptableObject.CreateInstance<BehaviourTreeUI.TreeGraph>();
        uiTree.Tree = target as AI.BehaviorTree;
        uiTree.CreateTree();

        string editorPath = "Assets/Editor";
        string treeFolder = "TreeGraphs";
        string treePath = editorPath + "/" + treeFolder;
        if (!AssetDatabase.IsValidFolder(treePath))
        {
            AssetDatabase.CreateFolder(editorPath, treeFolder);
        }

        string fileName = sourceTree.name + " graph 1.asset";
        int assetNum = 2;
        while (AssetDatabase.IsMainAssetAtPathLoaded(treePath + "/" + fileName))
        {
            fileName = fileName.Replace(" " + (assetNum - 1), " " + assetNum);
            assetNum++;
        }

        AssetDatabase.CreateAsset(uiTree, treePath + "/" + fileName);
        AssetDatabase.SaveAssets();

        return uiTree;
    }
}
