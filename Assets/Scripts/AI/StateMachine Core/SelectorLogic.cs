using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
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

        public bool Evaluate(Blackboard bb)
        {
            if (bb.Properties.ContainsKey(PropertyOneToEvaluate))
            {
                return EvaluateByType(bb.Properties[PropertyOneToEvaluate], GetPropertyTwoValue(bb)); 
            }

            return false;
        }

        protected bool EvaluateByType(object obj1, object obj2)
        {
            if(obj1 == null || obj2 == null)
            {
                return false;
            }

            if (obj1 is bool)
            {
                return EvaluateWithComparisonMode((bool)obj1, (bool)obj2);
            }
            else if (obj1 is int)
            {
                return EvaluateWithComparisonMode((int)obj1, (int)obj2);
            }
            else if (obj1 is float)
            {
                return EvaluateWithComparisonMode((float)obj1, (float)obj2);
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

        protected object GetPropertyTwoValue(Blackboard bb)
        {
            if(PropertyTwoToEvaluate.StartsWith("(bool)"))
            {
                bool value;
                if(bool.TryParse(PropertyTwoToEvaluate.Substring(6), out value))
                {
                    return value;
                }
                return null;
            }
            else if(PropertyTwoToEvaluate.StartsWith("(int)"))
            {
                int value;
                if (int.TryParse(PropertyTwoToEvaluate.Substring(5), out value))
                {
                    return value;
                }
                return null;
            }
            else if(PropertyTwoToEvaluate.StartsWith("(float)"))
            {
                float value;
                if (float.TryParse(PropertyTwoToEvaluate.Substring(7), out value))
                {
                    return value;
                }
                return null;
            }

            if (bb == null) { return null; }
            return bb.Properties[PropertyTwoToEvaluate];
        }
    }
}