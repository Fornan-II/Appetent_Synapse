using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public AI.BehaviorTree myTree;
    public AI.Blackboard localBlackboard;// = new AI.Blackboard();
    public int treeUpdateInterval = 64;

    private int _treeTicks = 0; 

    protected virtual void FixedUpdate()
    {
        if(myTree && (localBlackboard != null))
        {
            if (_treeTicks <= 0)
            {
                myTree.ProcessTree(localBlackboard);
                _treeTicks = treeUpdateInterval;
            }
            _treeTicks--;
        }
    }
}
