using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Node : ScriptableObject
    {
        protected virtual void OnEnable()
        {
            //hideFlags = HideFlags.DontSaveInEditor;
        }

        //Returns true if node finishes processing during call
        public abstract bool Process(BehaviorTree tree);
    }
}
