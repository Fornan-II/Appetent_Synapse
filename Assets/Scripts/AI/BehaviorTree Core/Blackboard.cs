using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Blackboard
    {
        //NodesToProcess used by BehaviorTree
        [HideInInspector]public Stack<Node> NodesToProcess = new Stack<Node>();
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

        public BlackBoardDictionary Properties;
        
        public Blackboard()
        {
            Properties = new BlackBoardDictionary();
        }

        public virtual object GetProperty(string propertyName)
        {
            if(Properties.ContainsKey(propertyName))
            {
                return Properties[propertyName];
            }

            return null;
        }

        public virtual void SetProperty(string propertyName, object value = null)
        {
            if (Properties.ContainsKey(propertyName))
            {
                Properties[propertyName] = value;
            }
            else
            {
                Properties.Add(propertyName, value);
            }
        }

        public virtual void RemoveProperty(string propertyName)
        {
            Properties.Remove(propertyName);
        }

        public virtual System.Type GetPropertyType(string propertyName)
        {
            return GetProperty(propertyName).GetType();

        }
    }
}