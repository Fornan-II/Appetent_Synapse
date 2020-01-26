using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI.BehaviorTree;

namespace AI
{
    [CreateAssetMenu(fileName = "New AI", menuName = "BehaviorTree/New Behavior Tree")]
    public class BehaviorTreeAsset : ScriptableObject
    {
        [SerializeField] protected Root _treeRoot;
        public Root TreeRoot { get { return _treeRoot; } }

        private void OnEnable()
        {
            _treeRoot = new Root();
            //_treeRoot = new AI.BehaviorTree.Root(
            //new AI.BehaviorTree.Selector(
            //    new AI.BehaviorTree.Sequence(new Node[]
            //    {
            //        new AI.BehaviorTree.Leaf(Behaviors.MoveToTarget),
            //        new AI.BehaviorTree.Leaf(Behaviors.MeleeAttack)
            //    }),
            //    new AI.BehaviorTree.Leaf(Behaviors.Patrol),
            //    new AI.BehaviorTree.SelectorLogic("IsAggro", "(bool)true", AI.BehaviorTree.SelectorLogic.ComparisonMode.EQUAL)
            //    )
            //);
        }
    }
}