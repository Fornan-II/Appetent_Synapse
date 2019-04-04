using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public class TreeGraph : Graph
    {
        public AI.BehaviorTree Tree;
        [SerializeField]protected BehaviorNode _rootNode;
        public string graphNodeFolderPath;
        protected bool _treeIsValid = false;
        protected string validationErrorMessage = "";

        public virtual void CreateTree()
        {
            if(!Tree)
            {
                Debug.LogWarning("Tree graph attempting to be created without a behavior tree object assigned");
                return;
            }

            CreateNode(Tree.root, Vector2.zero);
        }

        protected virtual Slot CreateNode(AI.Node n, Vector2 pos, bool saveToDatabase = true)
        {
            if (n is AI.Root)
            {
                AI.Root r = n as AI.Root;
                GraphRoot dispNode = BehaviorNode.NewRoot().node as GraphRoot;
                if (saveToDatabase)
                {
                    SaveGraphNodeAsset(dispNode, n.name);
                }
                dispNode.sourceNode = r;
                dispNode.position.position = pos;

                if (r.NextNode)
                {
                    Slot nextInputSlot = CreateNode(r.NextNode, pos + new Vector2(150, 0), saveToDatabase);

                    Connect(dispNode.slots[0], nextInputSlot);
                }

                if (_rootNode)
                {
                    Debug.LogWarning("Tree Graph seems to already have root node? Overwriting.");
                }
                _rootNode = dispNode;

                AddNode(dispNode);
                return null;
            }
            else if (n is AI.Selector)
            {
                AI.Selector sel = n as AI.Selector;
                NodeInfo nodeInfo = BehaviorNode.NewSelector();
                GraphSelector dispNode = nodeInfo.node as GraphSelector;
                if (saveToDatabase)
                {
                    SaveGraphNodeAsset(dispNode, n.name);
                }
                dispNode.sourceNode = sel;
                dispNode.position.position = pos;

                if (sel.Logic != null)
                {
                    dispNode.PropertyOneToEvaluate = sel.Logic.PropertyOneToEvaluate;

                    string propertyTwo = sel.Logic.PropertyTwoToEvaluate;
                    if (propertyTwo[0] == '(')
                    {
                        int typeEndIndex = propertyTwo.IndexOf(')');
                        switch (propertyTwo.Substring(0, typeEndIndex + 1))
                        {
                            case "(bool)":
                                {
                                    dispNode.PropertyTwoToEvaluate = propertyTwo.Substring(typeEndIndex + 1);
                                    dispNode.PropertyTwoType = GraphSelector.PropertyType.BOOL;
                                    break;
                                }
                            case "(float)":
                                {
                                    dispNode.PropertyTwoToEvaluate = propertyTwo.Substring(typeEndIndex + 1);
                                    dispNode.PropertyTwoType = GraphSelector.PropertyType.FLOAT;
                                    break;
                                }
                            case "(int)":
                                {
                                    dispNode.PropertyTwoToEvaluate = propertyTwo.Substring(typeEndIndex + 1);
                                    dispNode.PropertyTwoType = GraphSelector.PropertyType.INT;
                                    break;
                                }
                            default:
                                {
                                    Debug.LogWarning("Logic property 2 starts with \'(\' but isn't a type. Try not to use parantheses in property names.");
                                    dispNode.PropertyTwoToEvaluate = propertyTwo;
                                    dispNode.PropertyTwoType = GraphSelector.PropertyType.BLACKBOARD;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        dispNode.PropertyTwoType = GraphSelector.PropertyType.BLACKBOARD;
                        dispNode.PropertyTwoToEvaluate = sel.Logic.PropertyTwoToEvaluate;
                    }

                    dispNode.Mode = sel.Logic.Mode;
                }

                Slot o = dispNode.AddOutputSlot("true:");
                if (sel.nodeOnTrue)
                {
                    Slot nextInputSlot = CreateNode(sel.nodeOnTrue, pos + new Vector2(150, -50), saveToDatabase);
                    Connect(o, nextInputSlot);
                }

                o = dispNode.AddOutputSlot("false:");
                if (sel.nodeOnFalse)
                {
                    Slot nextInputSlot = CreateNode(sel.nodeOnFalse, pos + new Vector2(150, 50), saveToDatabase);
                    Connect(o, nextInputSlot);
                }

                AddNode(dispNode);
                return nodeInfo.inSlot;
            }
            else if(n is AI.Sequence)
            {
                AI.Sequence seq = n as AI.Sequence;
                NodeInfo nodeInfo = BehaviorNode.NewSequence();
                GraphSequence dispNode = nodeInfo.node as GraphSequence;
                if (saveToDatabase)
                {
                    SaveGraphNodeAsset(dispNode, n.name);
                }
                dispNode.sourceNode = seq;
                dispNode.position.position = pos;

                int outName = 0;
                if(seq.sequenceNodes != null)
                {
                    float verticalOffset = 50.0f - ((seq.sequenceNodes.Length * 100.0f) / 2.0f);
                    foreach (AI.Node child in seq.sequenceNodes)
                    {
                        Slot o = dispNode.AddOutputSlot("out:" + outName);
                        Slot nextInputSlot = CreateNode(child, pos + new Vector2(150, verticalOffset), saveToDatabase);

                        Connect(o, nextInputSlot);

                        outName++;
                        verticalOffset += 100.0f;
                    }
                }

                AddNode(dispNode);
                return nodeInfo.inSlot;
            }
            else if(n is AI.Leaf)
            {
                AI.Leaf l = n as AI.Leaf;
                NodeInfo nodeInfo = BehaviorNode.NewLeaf();
                GraphLeaf dispNode = nodeInfo.node as GraphLeaf;
                if (saveToDatabase)
                {
                    SaveGraphNodeAsset(dispNode, n.name);
                }
                dispNode.sourceNode = l;
                //dispNode.node.SetPropertyValue("Behavior Phase", l.nodeBehavior.CurrentPhase);
                dispNode.position.position = pos;

                AddNode(dispNode);
                return nodeInfo.inSlot;
            }

            Debug.LogWarning("Something went wrong in TreeGraph.CreateNode()");
            return null;
        }

        protected virtual void SaveGraphNodeAsset(Node n, string assetName)
        {
            string fileName = assetName + ".asset";

            AssetDatabase.CreateAsset(n, graphNodeFolderPath + "/" + fileName);
            AssetDatabase.SaveAssets();
        }

        public virtual void Validate()
        {
            validationErrorMessage = "";
            _treeIsValid = true;

            if(!_rootNode)
            {
                _treeIsValid = false;
                validationErrorMessage += "\nNo root node";
                return;
            }

            foreach(BehaviorNode bn in nodes)
            {
                if(!bn.IsValid())
                {
                    validationErrorMessage += "\nNode " + bn.name + " is invalid";
                    _treeIsValid = false;
                }
            }
            
            //Show which tree node is active;
            if(EditorApplication.isPlaying && _treeIsValid)
            {
                BehaviorNode activeNode = FindActiveNode();
                if(activeNode)
                {
                    activeNode.color = Styles.Color.Green;
                }
            }
        }

        protected virtual BehaviorNode FindActiveNode()
        {
            if(Tree.currentAI == null)
            {
                return null;
            }

            AI.Node ActiveNode = Tree.currentAI.ActiveNode;
            if(ActiveNode == null)
            {
                ActiveNode = Tree.root;
            }
            foreach(BehaviorNode bn in nodes)
            {
                if (bn.GetAINode() == ActiveNode)
                {
                    return bn;
                }
            }
            return null;
        }

        #region Toolbar functionality
        public virtual void SaveGraphToSources()
        {
            if (!_treeIsValid)
            {
                Debug.Log("Cannot save graph - currently invalid." + validationErrorMessage);
                return;
            }

            _rootNode.SaveDataRecursive(Tree, SaveGraphNodeAsset);

            AssetDatabase.SaveAssets();
            Debug.Log("Graph saved.");
        }

        public virtual void CreateNewNode<T>(Vector2 pos) where T : AI.Node
        {
            AI.Node newNode = ScriptableObject.CreateInstance<T>();
            Slot s = CreateNode(newNode, pos, false);
            BehaviorNode bn = s.node as BehaviorNode;
            bn.Label = newNode.GetType().ToString().Substring(3);
            bn.title = bn.Label;
        }
        #endregion

        public override void DestroyNode(Node node)
        {
            if (AssetDatabase.Contains(node))
            {
                string graphNodePath = AssetDatabase.GetAssetPath(node);

                if (node is BehaviorNode)
                {
                    BehaviorNode bn = node as BehaviorNode;
                    AI.Node ai = bn.GetAINode();
                    if (ai)
                    {
                        string AINodePath = AssetDatabase.GetAssetPath(ai);
                        AssetDatabase.DeleteAsset(AINodePath);
                    }

                }

                base.DestroyNode(node);
                AssetDatabase.DeleteAsset(graphNodePath);
            }
            else
            {
                base.DestroyNode(node);
            }
        }
    }
}