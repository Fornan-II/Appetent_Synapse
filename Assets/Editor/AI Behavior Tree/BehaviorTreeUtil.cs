using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;


namespace BehaviourTreeUI
{
    public static class BehaviorTreeUtil
    {
        public static Type[] GetClasses()
        {
            //https://answers.unity.com/questions/363644/get-all-inherited-classes-of-an-abstract-class-edi.html
            Assembly assemblyWithBehaviors = Assembly.GetAssembly(typeof(AI.Behavior));
            Type[] types = assemblyWithBehaviors.GetTypes();
            Type[] possible = (from Type type in types where type.IsSubclassOf(typeof(AI.Behavior)) select type).ToArray();

            return possible;

            //string[] typeNames = new string[possible.Count()];
            //int i = 0;
            //foreach (Type t in possible)
            //{
            //    typeNames[i] = t.ToString();
            //    i++;
            //}
            //return typeNames;
        }
    }
}