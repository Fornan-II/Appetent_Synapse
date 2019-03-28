using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://answers.unity.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html
[System.Serializable]
public class BlackBoardDictionary : Dictionary<string, object>, ISerializationCallbackReceiver
{
    private struct ValueInfo
    {
        public ValueType Type;
        public string SerializableValue;

        public ValueInfo(ValueType T, string V)
        {
            Type = T;
            SerializableValue = V;
        }
    }

    public enum ValueType
    {
        FLOAT,
        INT,
        BOOL,
        OTHER
    }


    [SerializeField]
    private List<string> keys = new List<string>();

    [SerializeField]
    private List<ValueType> valueTypes = new List<ValueType>();

    [SerializeField]
    private List<string> serializedValues = new List<string>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        valueTypes.Clear();
        serializedValues.Clear();
        foreach (KeyValuePair<string, object> pair in this)
        {
            keys.Add(pair.Key);
            ValueInfo val = GetValueInfo(pair.Value);
            valueTypes.Add(val.Type);
            serializedValues.Add(val.SerializableValue);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        if (keys.Count != valueTypes.Count && keys.Count != serializedValues.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        Dictionary<string, object> replacementDict = new Dictionary<string, object>();

        foreach(KeyValuePair<string, object> kvp in this)
        {
            if(keys.Contains(kvp.Key))
            {
                int i = keys.IndexOf(kvp.Key);
                object value;
                if (TryGetObjectOf(valueTypes[i], serializedValues[i], out value))
                {
                    replacementDict.Add(keys[i], value);
                }
                else
                {
                    replacementDict.Add(kvp.Key, kvp.Value);
                }
            }
        }

        this.Clear();
        foreach (KeyValuePair<string, object> kvp in this)
        {
            this.Add(kvp.Key, kvp.Value);
        }
    }

    private bool TryGetObjectOf(ValueType type, string serializedValue, out object result)
    {
        result = null;
        switch(type)
        {
            case ValueType.FLOAT:
                {
                    float f;
                    if(float.TryParse(serializedValue, out f))
                    {
                        result = f;
                        return true;
                    }
                    return false;
                }
            case ValueType.INT:
                {
                    int i;
                    if(int.TryParse(serializedValue, out i))
                    {
                        result = i;
                        return true;
                    }
                    return false;
                }
            case ValueType.BOOL:
                {
                    bool b;
                    if(bool.TryParse(serializedValue, out b))
                    {
                        result = b;
                        return true;
                    }
                    return false;
                }
            default:
                {
                    return false;
                }
        }
    }

    private ValueInfo GetValueInfo(object value)
    {
        if (value is float)
        {
            return new ValueInfo(ValueType.FLOAT, value.ToString());
        }
        else if (value is int)
        {
            return new ValueInfo(ValueType.INT, value.ToString());
        }
        else if (value is bool)
        {
            return new ValueInfo(ValueType.BOOL, value.ToString());
        }
        else
        {
            return new ValueInfo(ValueType.OTHER, "(" + value.GetType() + ")");
        }
    }
}
