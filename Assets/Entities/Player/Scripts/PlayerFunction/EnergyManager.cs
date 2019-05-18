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
    public int EnergyUpdateInterval = 240;

    public Modifier DrainRate = new Modifier(0.0f, Modifier.CalculateMode.ADD);

    public bool ProcessEnergy = true;

    protected int _energyTicks = 0;

    public EnergyEvent OnProcessEnergy;
    public EnergyEvent OnProcessExcessEnergy;
    public IntEvent OnEnergyValueChange;

    protected virtual void FixedUpdate()
    {
        if(ProcessEnergy)
        {
            if (_energy > MaxEnergy)
            {
                OnProcessExcessEnergy.Invoke(this);
            }

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
        
        OnProcessEnergy.Invoke(this);

        AddEnergy(-DrainRate.Value);
    }

    public virtual void AddEnergy(float value)
    {
        if (value != 0)
        {
            _energy += value;
            _energy = Mathf.Clamp(_energy, 0.0f, MaxEnergy + MaxExcessEnergy);

            OnEnergyValueChange.Invoke(Mathf.CeilToInt(_energy));
        }
    }
}
