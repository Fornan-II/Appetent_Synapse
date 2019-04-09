using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
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

        [HideInInspector] public Behavior behaviorInstance;
        [HideInInspector] public Dictionary<Sequence, int> instanceSequencePositions = new Dictionary<Sequence, int>();
        #endregion

        public AI.BehaviorTree myTree;
        public AIPawn aiPawn;
        public AI.Blackboard localBlackboard;// = new AI.Blackboard();
        public int treeUpdateInterval = 64;

        private int _treeTicks = 0;

        public string debugOutput;

        protected virtual void Start()
        {
            localBlackboard = new AI.Blackboard();
            if(aiPawn)
            {
                aiPawn.Init(this);
            }
            else
            {
                Debug.LogWarning(name + " does not have an aiPawn assigned? This will cause errors.");
            }
        }

        protected virtual void FixedUpdate()
        {
            if(behaviorInstance) { debugOutput = behaviorInstance.ToString() + " | " + behaviorInstance.CurrentPhase; }
            else { debugOutput = "null"; }

            if (myTree && (localBlackboard != null))
            {
                if (_treeTicks <= 0)
                {
                    myTree.ProcessTree(this);
                    _treeTicks = treeUpdateInterval;
                }
                _treeTicks--;
            }
        }

        public virtual void InterruptBehavior()
        {
            Node currentNode = ActiveNode;
            NodesToProcess.Clear();
            instanceSequencePositions.Clear();

            if(currentNode is Leaf)
            {
                NodesToProcess.Push(currentNode);
                (currentNode as Leaf).ForceBehaviorToEnd(this);
            }
        }
    }
}