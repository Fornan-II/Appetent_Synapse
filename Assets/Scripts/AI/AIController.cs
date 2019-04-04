using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class AIController : MonoBehaviour
{
    #region Stuff used for instance management of Behavior Tree & Behaviors
    //NodesToProcess used by BehaviorTree
    public Stack<Node> NodesToProcess = new Stack<Node>();
    //On the stack:
    //1 : ActiveLeaf
    //2 : Any Sequences
    //If nothing, return to root
    public Node ActiveNode
    {
        get
        {
            if (NodesToProcess == null || NodesToProcess.Count <= 0)
            {
                return null;
            }
            else
            {
                return NodesToProcess.Peek();
            }
        }
    }

    public Node[] debugStack;

    [HideInInspector] public Behavior behaviorInstance;
    [HideInInspector] public Dictionary<Sequence, int> instanceSequencePositions = new Dictionary<Sequence, int>();
    #endregion

    public AI.BehaviorTree myTree;
    public AI.Blackboard localBlackboard;// = new AI.Blackboard();
    public int treeUpdateInterval = 64;

    private int _treeTicks = 0;

    public string debugOutput;

    private void Start()
    {
        localBlackboard = new AI.Blackboard();
        localBlackboard.SetProperty("IsAggrod", false);
    }

    protected virtual void FixedUpdate()
    {
        debugStack = NodesToProcess.ToArray();

        if(myTree && (localBlackboard != null))
        {
            if (_treeTicks <= 0)
            {
                myTree.ProcessTree(this);
                _treeTicks = treeUpdateInterval;
            }
            _treeTicks--;
        }
    }
}
