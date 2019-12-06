using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI.BehaviorTree;

namespace AI
{
    [CreateAssetMenu(fileName = "New AI", menuName = "BehaviorTree/New Behavior Tree")]
    public class BehaviorTreeAsset : ScriptableObject
    {
        [SerializeField] protected Root _treeRoot = new Root();
        public Root TreeRoot { get { return _treeRoot; } }
    }
}