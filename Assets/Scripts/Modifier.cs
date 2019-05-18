using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier
{
    [SerializeField]protected float _baseValue;
    public float BaseValue
    {
        get
        {
            return _baseValue;
        }
        set
        {
            _baseValue = value;
            RefreshCachedValue();
        }
    }

    [SerializeField]protected float _value;
    public float Value
    {
        get
        {
            return _value;
        }
    }

    protected Dictionary<string, float> _modifiers;

    public Modifier(float baseValue)
    {
        _baseValue = baseValue;
        _modifiers = new Dictionary<string, float>();
        RefreshCachedValue();
    }

    public Modifier(float baseValue, Dictionary<string, float> modifiers)
    {
        _baseValue = baseValue;
        _modifiers = modifiers;
        RefreshCachedValue();
    }

    public virtual void SetModifier(string key, float value)
    {
        if (_modifiers == null)
        {
            _modifiers = new Dictionary<string, float>();
        }

        if (_modifiers.ContainsKey(key))
        {
            if(_modifiers[key] != value)
            {
                _modifiers[key] = value;
                RefreshCachedValue();
            }
        }
        else
        {
            _modifiers.Add(key, value);
            RefreshCachedValue();
        }
    }

    public virtual void RemoveModifier(string key)
    {
        if (_modifiers == null) { return; }

        if (_modifiers.ContainsKey(key))
        {
            _modifiers.Remove(key);
            RefreshCachedValue();
        }
    }

    public virtual void ClearModifiers()
    {
        if (_modifiers == null) { return; }

        if (_modifiers.Count > 0)
        {
            _modifiers.Clear();
            RefreshCachedValue();
        }
    }

    protected virtual void RefreshCachedValue()
    {
        _value = _baseValue;

        if (_modifiers != null)
        {
            foreach (float modifier in _modifiers.Values)
            {
                _value *= modifier;
            }
        }
    }
}
