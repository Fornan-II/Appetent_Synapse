using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class SelectorLogic
    {
        public string PropertyOneToEvaluate;
        public string PropertyTwoToEvaluate;

        

        public enum ComparisonMode
        {
            LESS_THAN,
            LESS_THAN_EQUAL,
            GREATER_THAN,
            GREATER_THAN_EQUAL,
            EQUAL,
            NOT_EQUAL
        }
        public ComparisonMode Mode = ComparisonMode.EQUAL;

        public bool Evaluate(Blackboard bbThis, Blackboard bbOther)
        {
            if (bbThis.Properties.ContainsKey(PropertyOneToEvaluate))
            {
                if(bbThis.Properties[PropertyOneToEvaluate] is bool)
                return (bool)bbThis.Properties[PropertyOneToEvaluate];
            }

            return false;
        }

        protected bool EvaluateByType(object o)
        {
            if (o is bool)
            {
                return EvaluateWithComparisonMode((bool)o, true);
            }
            else if (o is int)
            {
                return EvaluateWithComparisonMode((int)o, 0);
            }
            else if (o is float)
            {
                return EvaluateWithComparisonMode((float)o, 0);
            }
            else
            {
                Debug.LogWarning("Attempting to evalutate unhandled type - returning false.");
                return false;
            }
        }

        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-methods
        protected bool EvaluateWithComparisonMode<T>(T valueOne, T valueTwo) where T : System.IComparable<T>
        {
            switch(Mode)
            {
                case ComparisonMode.LESS_THAN:
                    {
                        return valueOne.CompareTo(valueTwo) < 0;
                    }
                case ComparisonMode.LESS_THAN_EQUAL:
                    {
                        return valueOne.CompareTo(valueTwo) <= 0;
                    }
                case ComparisonMode.GREATER_THAN:
                    {
                        return valueOne.CompareTo(valueTwo) > 0;
                    }
                case ComparisonMode.GREATER_THAN_EQUAL:
                    {
                        return valueOne.CompareTo(valueTwo) >= 0;
                    }
                case ComparisonMode.EQUAL:
                    {
                        return valueOne.CompareTo(valueTwo) == 0;
                    }
                case ComparisonMode.NOT_EQUAL:
                    {
                        return valueOne.CompareTo(valueTwo) != 0;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        //protected object GetPropertyTwoValue(Blackboard bb)
        //{
        //    if(!bb)
        //    {
        //        return 
        //    }
        //}
    }
}