using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviorTree
{
    public abstract class Node
    {
        public abstract void Process(BehaviorTree tree);
    }
}
