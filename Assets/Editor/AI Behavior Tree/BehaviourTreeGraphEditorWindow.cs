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

        [MenuItem("Window/Behaviour Tree")]
        public static void Do()
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();
        }

        #region Graph Creation
        public static void DoAITree(AI.BehaviorTree tree)
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();

            graphEditorWindow.behaviourTreeGraph = ScriptableObject.CreateInstance<TreeGraph>();
            graphEditorWindow.behaviourTreeGraph.Tree = tree;
            graphEditorWindow.behaviourTreeGraph.CreateTree();
            graphEditorWindow.behaviourTreeGraphGUI = ScriptableObject.CreateInstance<GraphGUIEX>();
            graphEditorWindow.behaviourTreeGraphGUI.graph = graphEditorWindow.behaviourTreeGraph;

            graphEditorWindow._toolbarItems.Clear();
            AddToolBarItem("Save Tree", graphEditorWindow.behaviourTreeGraph.SaveGraphToSources);
            AddToolBarItem("New Leaf Node", graphEditorWindow.CreateNewLeaf);
            AddToolBarItem("New Selector Node", graphEditorWindow.CreateNewSelector);
            AddToolBarItem("New Sequence Node", graphEditorWindow.CreateNewSequence);
        }

        public static void DoTree(TreeGraph tree)
        {
            graphEditorWindow = GetWindow<BehaviourTreeGraphEditorWindow>();

            graphEditorWindow.behaviourTreeGraph = tree;
            graphEditorWindow.behaviourTreeGraphGUI = ScriptableObject.CreateInstance<GraphGUIEX>();
            graphEditorWindow.behaviourTreeGraphGUI.graph = graphEditorWindow.behaviourTreeGraph;

            graphEditorWindow._toolbarItems.Clear();
            AddToolBarItem("Save Tree", graphEditorWindow.behaviourTreeGraph.SaveGraphToSources);
            AddToolBarItem("New Leaf Node", graphEditorWindow.CreateNewLeaf);
            AddToolBarItem("New Selector Node", graphEditorWindow.CreateNewSelector);
            AddToolBarItem("New Sequence Node", graphEditorWindow.CreateNewSequence);
        }
        #endregion

        #region Toolbar Management
        public delegate void ToolBarAction();
        int _selectedToolbarItem = -1;
        protected List<KeyValuePair<string, ToolBarAction>> _toolbarItems = new List<KeyValuePair<string, ToolBarAction>>();

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
        #endregion

        #region Toolbar Function
        protected void CreateNewLeaf()
        {
            if(!graphEditorWindow)
            {
                return;
            }

            graphEditorWindow.behaviourTreeGraph.CreateNewNode<AI.Leaf>(graphEditorWindow.behaviourTreeGraphGUI.GetCenterPosition());
        }

        protected void CreateNewSelector()
        {
            graphEditorWindow.behaviourTreeGraph.CreateNewNode<AI.Selector>(graphEditorWindow.behaviourTreeGraphGUI.GetCenterPosition());
        }

        protected void CreateNewSequence()
        {
            graphEditorWindow.behaviourTreeGraph.CreateNewNode<AI.Sequence>(graphEditorWindow.behaviourTreeGraphGUI.GetCenterPosition());
        }
        #endregion

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

                behaviourTreeGraphGUI.BeginGraphGUI(graphEditorWindow, new Rect(0, 30, graphEditorWindow.position.width, graphEditorWindow.position.height - 30));

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