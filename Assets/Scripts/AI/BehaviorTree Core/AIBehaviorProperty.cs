using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;


namespace AI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AIBehavior : System.Attribute
    {
        public static string[] GetClassNames()
        {
            //https://stackoverflow.com/questions/28509317/get-all-types-that-implement-an-interface-in-unity
            Type type = typeof(Behavior);
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.StartsWith("AI"))
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && type.IsAssignableFrom(x));

            string[] typeNames = new string[types.Count()];
            int i = 0;
            foreach(Type t in types)
            {
                typeNames[i] = t.ToString();
                i++;
            }
            return typeNames;

            //Attribute[] allAttributes = GetCustomAttributes(typeof(AIBehavior), true);
            //List<string> classNames = new List<string>();
            //foreach (Attribute att in allAttributes)
            //{
            //    classNames.Add(att.ToString());
            //}

            //return classNames.ToArray();
        }
    }

    [AIBehavior]
    public class Foo
    {

    }
}