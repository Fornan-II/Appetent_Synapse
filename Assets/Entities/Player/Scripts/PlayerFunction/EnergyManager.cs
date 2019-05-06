using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField]protected float _energy = 20;
    public int Energy
    {
        get
        {
            return Mathf.Min(MaxEnergy, Mathf.CeilToInt(_energy));
        }
    }
    public int ExcessEnergy
    {
        get
        {
            return Mathf.Max(0, Mathf.CeilToInt(_energy - MaxEnergy));
        }
    }

    public int MaxEnergy = 20;
    public int MaxExcessEnergy = 5;
    public int EnergyEffectThreshold = 16;
    public int EnergyUpdateInterval = 240;

    public float BaseDrainRate = 0.0f;
    public Queue<float> DrainModifiers = new Queue<float>();

    public bool ProcessEnergy = true;

    protected int _energyTicks = 0;

    public EnergyEvent OnProcessEnergyEffect;
    public EnergyEvent OnProcessExcessEnergy;

    protected virtual void FixedUpdate()
    {
        if(ProcessEnergy)
        {
            if (_energyTicks <= 0)
            {
                Process();
                _energyTicks = EnergyUpdateInterval;
            }
            _energyTicks--;
        }
    }

    protected virtual void Process()
    {
        _energy = Mathf.Clamp(_energy, 0.0f, MaxEnergy + MaxExcessEnergy);

        if(_energy > MaxEnergy)
        {
            OnProcessExcessEnergy.Invoke(this);
        }
        else if(_energy > EnergyEffectThreshold)
        {
            OnProcessEnergyEffect.Invoke(this);
        }

        float drainAmount = BaseDrainRate;
        while(DrainModifiers.Count > 0)
        {
            drainAmount += DrainModifiers.Dequeue();
        }
        AddEnergy(-drainAmount);
    }

    public virtual void AddEnergy(float value)
    {
        _energy += value;
        _energy = Mathf.Clamp(_energy, 0.0f, MaxEnergy + MaxExcessEnergy);
    }
}
