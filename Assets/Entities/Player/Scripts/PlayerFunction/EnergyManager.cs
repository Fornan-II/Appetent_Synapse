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

    public EnergyEvent OnProcessEnergy;
    public IntEvent OnEnergyValueChange;

    protected virtual void FixedUpdate()
    {
        if(ProcessEnergy)
        {
            Process(Time.fixedDeltaTime);
        }
    }

    protected virtual void Process(float deltaTime)
    {
        _energy = Mathf.Clamp(_energy, 0.0f, MaxEnergy + MaxExcessEnergy);

        float energyDrained = DrainRate.Value * deltaTime;

        OnProcessEnergy.Invoke(this, energyDrained);

        AddEnergy(-energyDrained);
    }

    public virtual void AddEnergy(float value)
    {
        if (value != 0)
        {
            int oldEnergy = Mathf.CeilToInt(_energy);

            _energy += value;
            _energy = Mathf.Clamp(_energy, 0.0f, MaxEnergy + MaxExcessEnergy);

            int newEnergy = Mathf.CeilToInt(_energy);
            if (newEnergy != oldEnergy)
            {
                OnEnergyValueChange.Invoke(newEnergy);
            }
        }
    }
}
