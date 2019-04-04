using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "Leaf 1", menuName = "BehaviorTree/Nodes/New Leaf")]
    public class Leaf : Node
    {
        [SerializeField]protected Behavior _nodeBehavior;
        public Behavior NodeBehavior
        {
            get
            {
                return _nodeBehavior;
            }
            set
            {
                if(UnityEditor.AssetDatabase.Contains(this))
                {
                    if (_nodeBehavior)
                    {
                        UnityEditor.AssetDatabase.RemoveObjectFromAsset(_nodeBehavior);
                    }
                    _nodeBehavior = value;
                    UnityEditor.AssetDatabase.AddObjectToAsset(_nodeBehavior, this);
                }
                else
                {
                    _nodeBehavior = value;
                }
            }
        }

        protected Behavior.StatePhase _previousPhase = Behavior.StatePhase.INACTIVE;

        public override bool Process(BehaviorTree tree)
        {
            if(_nodeBehavior == null)
            {
                Debug.LogWarning(name + " is leaf node with unassigned nodeBehavior");
                return true;
            }

            //This will only be true when the node is first used by the currentAI
            if(tree.currentAI.behaviorInstance == null)
            {
                tree.QueueNode(this);
                tree.currentAI.behaviorInstance = ScriptableObject.CreateInstance(_nodeBehavior.GetType()) as Behavior;
                _previousPhase = Behavior.StatePhase.INACTIVE;
            }

            Behavior.StatePhase stateOnProcessing = tree.currentAI.behaviorInstance.CurrentPhase;
            //Debug.Log(name + " processing " + tree.currentAI.behaviorInstance + " with phase " + tree.currentAI.behaviorInstance.CurrentPhase + "\nPrev:" + _previousPhase + " for " + tree.currentAI.name);

            switch (tree.currentAI.behaviorInstance.CurrentPhase)
            {
                case Behavior.StatePhase.ENTERING:
                    {
                        if (_previousPhase != tree.currentAI.behaviorInstance.CurrentPhase)
                        {
                            tree.currentAI.behaviorInstance.OnEnter(tree.currentAI);
                        }
                        else
                        {
                            tree.currentAI.behaviorInstance.EnterBehavior(tree.currentAI);
                        }
                        break;
                    }
                case Behavior.StatePhase.ACTIVE:
                    {
                        tree.currentAI.behaviorInstance.ActiveBehavior(tree.currentAI);
                        break;
                    }
                case Behavior.StatePhase.EXITING:
                    {
                        if (_previousPhase != tree.currentAI.behaviorInstance.CurrentPhase)
                        {
                            tree.currentAI.behaviorInstance.OnExit(tree.currentAI);
                        }
                        else
                        {
                            tree.currentAI.behaviorInstance.ExitBehavior(tree.currentAI);
                        }
                        break;
                    }
                case Behavior.StatePhase.INACTIVE:
                    {
                        Destroy(tree.currentAI.behaviorInstance);
                        _previousPhase = Behavior.StatePhase.INACTIVE;
                        return true;
                    }
            }

            _previousPhase = stateOnProcessing;
            return false;
        }

        //This is going to have to change.
        public virtual void ForceBehaviorToEnd()
        {
            if(_nodeBehavior != null)
            {
                _nodeBehavior.ForceEndState();
            }
        }
    }
}