using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviorTree
{
    [System.Serializable]
    public class Node
    {
        public virtual void Process(BehaviorTree tree)
        {
            throw new System.NotImplementedException("Calling process from base type Node is not allowed.");
        }
    }
}
