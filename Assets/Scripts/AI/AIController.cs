using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIController : MonoBehaviour, IAIGroupMember
    {
        [SerializeField] protected BehaviorTree.BehaviorTree _behaviorTree = new BehaviorTree.BehaviorTree();
        public BehaviorTree.BehaviorTree behaviorTree { get { return _behaviorTree; } }
        public Blackboard Blackboard {
            get
            {
                if (_behaviorTree == null)
                    return null;
                return _behaviorTree.Blackboard;
            }
        }

        protected AIPawn _aiPawn;
        public AIPawn AIPawn
        {
            get { return _aiPawn; }
            set
            {
                if(_aiPawn != value)
                {
                    _aiPawn = value;
                    SetUpAIPawn();
                }
            }
        }

        [SerializeField] protected int treeUpdateInterval = 64;
        public bool ProcessTree = true;
        protected AIGroup _myGroup;

        private int _treeTicks = 0;

        public string debugOutput;

        protected virtual void Start()
        {
            if(_aiPawn)
            {
                SetUpAIPawn();
            }
            else
            {
                Debug.LogWarning(name + " does not have an aiPawn assigned? This will cause errors.");
            }
        }

        protected virtual void FixedUpdate()
        {
            if (_behaviorTree != null && _behaviorTree.Blackboard != null && ProcessTree)
            {
                if (_treeTicks <= 0)
                {
                    if (_behaviorTree.CurrentState.HasValue)
                    {
                        debugOutput = _behaviorTree.CurrentState.Value.ToString() + " | " + _behaviorTree.GetPhase();
                    }
                    else { debugOutput = "null"; }

                    _behaviorTree.Process(Time.fixedDeltaTime * treeUpdateInterval);
                    _treeTicks = treeUpdateInterval;
                }
                _treeTicks--;
            }
        }

        public virtual void InterruptBehavior()
        {
            _behaviorTree.ForceEndState();
        }

        public void SetGroup(AIGroup group)
        {
            _myGroup = group;
        }

        public AIGroup GetGroup()
        {
            return _myGroup;
        }

        protected virtual void SetUpAIPawn()
        {
            if (_behaviorTree != null)
            {
                if (_aiPawn)
                {
                    _behaviorTree.Blackboard.SetProperty("aiPawn", _aiPawn);
                    _aiPawn.Init(this);
                }
                else
                {
                    _behaviorTree.Blackboard.RemoveProperty("aiPawn");
                }
            }
        }
    }
}