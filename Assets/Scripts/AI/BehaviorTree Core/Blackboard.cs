using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Blackboard
    {
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

        public virtual bool HasProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }
    }
}