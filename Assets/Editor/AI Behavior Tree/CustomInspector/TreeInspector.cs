using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI.BehaviorTree))]
public class TreeInspector : Editor
{
    const string editorPath = "Assets/Editor/AI Behavior Tree";
    const string treeFolder = "TreeGraphs";
    const string fileNameSuffix = "Graph";

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

        EditorGUILayout.Space();

        if (GUILayout.Button("Regenerate Graph"))
        {
            if(target)
            {
                if(target is AI.BehaviorTree)
                {
                    ResetGraph(target as AI.BehaviorTree);
                }
            }
        }
    }

    protected BehaviourTreeUI.TreeGraph GetTree(AI.BehaviorTree sourceTree)
    {
        string treePath = editorPath + "/" + treeFolder;
        string fileName = sourceTree.name + fileNameSuffix + ".asset";

        BehaviourTreeUI.TreeGraph treeAsset = AssetDatabase.LoadAssetAtPath<BehaviourTreeUI.TreeGraph>(treePath + "/" + fileName);

        if (treeAsset)
        {
            return treeAsset;
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

        string fileName = sourceTree.name + fileNameSuffix;

        AssetDatabase.CreateAsset(uiTree, treePath + "/" + fileName + ".asset");
        AssetDatabase.CreateFolder(treePath, fileName);
        AssetDatabase.SaveAssets();
        //

        //Generate tree
        //
        uiTree.Tree = target as AI.BehaviorTree;
        uiTree.graphNodeFolderPath = treePath + "/" + fileName;
        uiTree.CreateTree();
        //

        return uiTree;
    }
    protected void ResetGraph(AI.BehaviorTree sourceTree)
    {
        string treePath = editorPath + "/" + treeFolder;
        string treeFileName = sourceTree.name + fileNameSuffix;

        AssetDatabase.DeleteAsset(treePath + "/" + treeFileName);
        AssetDatabase.DeleteAsset(treePath + "/" + treeFileName + ".asset");
    }
}
