using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergizedDamageReciever : DamageReciever
{
    public bool LetHeal = true;
    public int MinimumEnergyForHeal = 16;
    public float BaseHealCost = 1.0f;
    public float ExcessEnergyHealCost = 3.0f;

    protected float _energyDrainedSinceLastHeal = 0.0f;

    public virtual void OnEnergyProcess(EnergyManager source, float energyDrained)
    {   
        if(_health < MaxHealth && LetHeal && source.Energy >= MinimumEnergyForHeal)
        {
            _energyDrainedSinceLastHeal += energyDrained;

            if(source.ExcessEnergy > 0.0f)
            {
                source.DrainRate.SetModifier("healCost", ExcessEnergyHealCost);
            }
            else
            {
                source.DrainRate.SetModifier("healCost", BaseHealCost);
                
            }

            if (_energyDrainedSinceLastHeal >= 1.0f)
            {
                AddHealth(1);
                _energyDrainedSinceLastHeal = 0.0f;
            }
        }
        else
        {
            source.DrainRate.RemoveModifier("healCost");
            _energyDrainedSinceLastHeal = 0.0f;
        }
    }
}
