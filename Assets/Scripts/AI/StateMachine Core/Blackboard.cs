using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Blackboard
    {
        public Dictionary<string, object> Properties;
        
        public Blackboard()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}