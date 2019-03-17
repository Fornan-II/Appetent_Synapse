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
        TreeGraph behaviourTreeGraph;
        GraphGUIEX behaviourTreeGraphGUI;

        public delegate void ToolBarAction();
        int _selectedToolbarItem = -1;
        protected List<KeyValuePair<string, ToolBarAction>> _toolbarItems = new List<KeyValuePair<string, ToolBarAction>>();

        [MenuItem("Window/Behaviour Tree")]
        public static void Do()
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();
        }

        public static void DoAITree(AI.BehaviorTree tree)
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();

            graphEditorWindow.behaviourTreeGraph = ScriptableObject.CreateInstance<TreeGraph>();
            graphEditorWindow.behaviourTreeGraph.Tree = tree;
            graphEditorWindow.behaviourTreeGraph.CreateTree();
            graphEditorWindow.behaviourTreeGraphGUI = ScriptableObject.CreateInstance<GraphGUIEX>();
            graphEditorWindow.behaviourTreeGraphGUI.graph = graphEditorWindow.behaviourTreeGraph;

            AddToolBarItem("Save Tree", graphEditorWindow.behaviourTreeGraph.SaveGraphToSources);
        }

        public static void DoTree(TreeGraph tree)
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();

            graphEditorWindow.behaviourTreeGraph = tree;
            graphEditorWindow.behaviourTreeGraphGUI = ScriptableObject.CreateInstance<GraphGUIEX>();
            graphEditorWindow.behaviourTreeGraphGUI.graph = graphEditorWindow.behaviourTreeGraph;

            AddToolBarItem("Save Tree", graphEditorWindow.behaviourTreeGraph.SaveGraphToSources);
        }

        public static void AddToolBarItem(string label, ToolBarAction function)
        {
            if (graphEditorWindow)
            {
                graphEditorWindow._toolbarItems.Add(new KeyValuePair<string, ToolBarAction>(label, function));
            }
        }

        public static void RemoveToolBarItem(string label)
        {
            if(graphEditorWindow)
            {
                for(int i = 0; i < graphEditorWindow._toolbarItems.Count; i++)
                {
                    if(graphEditorWindow._toolbarItems[i].Key == label)
                    {
                        graphEditorWindow._toolbarItems.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public static void RemoveToolBarItem(int index)
        {
            if (graphEditorWindow)
            {
                graphEditorWindow._toolbarItems.RemoveAt(index);
            }
        }

        private void OnGUI()
        {
            if(graphEditorWindow && behaviourTreeGraphGUI != null)
            {
                if(_selectedToolbarItem != -1)
                {
                    _toolbarItems[_selectedToolbarItem].Value.Invoke();
                    _selectedToolbarItem = -1;
                }

                List<string> items = new List<string>();
                foreach(KeyValuePair<string, ToolBarAction> kvp in _toolbarItems)
                {
                    items.Add(kvp.Key);
                }

                _selectedToolbarItem = GUI.Toolbar(new Rect(0, 0, graphEditorWindow.position.width, 30), _selectedToolbarItem, items.ToArray());

                behaviourTreeGraphGUI.BeginGraphGUI(graphEditorWindow, new Rect(0, 30, graphEditorWindow.position.width, graphEditorWindow.position.height));

                behaviourTreeGraphGUI.OnGraphGUI();

                behaviourTreeGraphGUI.EndGraphGUI();
            }
            else if(graphEditorWindow)
            {
                graphEditorWindow.Close();
            }
        }
    }
}