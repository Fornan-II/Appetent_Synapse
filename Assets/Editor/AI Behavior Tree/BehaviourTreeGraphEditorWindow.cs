using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

//Source: https://stackoverflow.com/questions/17593101/how-to-write-a-gui-editor-for-graph-or-tree-structures
namespace BehaviourTreeUI
{
    public class BehaviourTreeGraphEditorWindow : EditorWindow
    {
        static BehaviourTreeGraphEditorWindow graphEditorWindow;
        Graph behaviourTreeGraph;
        GraphGUIEX behaviourTreeGraphGUI;

        [MenuItem("Window/Behaviour Tree")]
        static void Do()
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();
        }

        void CreateGraph()
        {
            behaviourTreeGraph = ScriptableObject.CreateInstance<Graph>();
            behaviourTreeGraph.hideFlags = HideFlags.HideAndDontSave;

            //Create new node
            Node node1 = ScriptableObject.CreateInstance<Node>();
            node1.title = "mile2";
            node1.position = new Rect(400, 34, 300, 200);

            node1.AddInputSlot("input");
            Slot start = node1.AddOutputSlot("output"); 
            node1.AddProperty(new Property(typeof(System.Int32), "integer"));
            behaviourTreeGraph.AddNode(node1);

            //Create new node
            Node node2 = ScriptableObject.CreateInstance<Node>();
            node2.title = "mile";
            node2.position = new Rect(0, 0, 300, 200);

            Slot end = node2.AddInputSlot("input");
            node2.AddOutputSlot("output");
            node2.AddProperty(new Property(typeof(System.Int32), "integer"));
            behaviourTreeGraph.AddNode(node2);

            //Create edge
            behaviourTreeGraph.Connect(start, end);

            behaviourTreeGraphGUI = ScriptableObject.CreateInstance<GraphGUIEX>();
            behaviourTreeGraphGUI.graph = behaviourTreeGraph;
        }

        private void OnGUI()
        {
            if(graphEditorWindow && behaviourTreeGraphGUI != null)
            {
                behaviourTreeGraphGUI.BeginGraphGUI(graphEditorWindow, new Rect(0, 0, graphEditorWindow.position.width, graphEditorWindow.position.height));

                behaviourTreeGraphGUI.OnGraphGUI();

                behaviourTreeGraphGUI.EndGraphGUI();
            }
            else
            {
                CreateGraph();
            }
        }
    }
}