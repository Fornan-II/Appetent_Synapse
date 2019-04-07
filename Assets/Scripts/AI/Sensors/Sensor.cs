using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIListener : MonoBehaviour
    {
        public AIController controller;

        public string AssociatePropertyName;

        public virtual void Alert(object value)
        {
            if(controller)
            {
                controller.localBlackboard.SetProperty(AssociatePropertyName, value);
                controller.InterruptBehavior();
            }
        }
    }
}