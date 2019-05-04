using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergizedDamageReciever : DamageReciever
{
    protected Coroutine _overHealRoutine;

    public float ExcessEnergyHealRate = 0.5f;
    public int ExcessEnergyHealCost = 3;

    public virtual void OnProcessEnergyEffect(EnergyManager source)
    {
        if(Health < MaxHealth)
        {
            AddHealth(1);
            source.DrainModifiers.Enqueue(1.0f);
        }
    }

    public virtual void OnProcessExcessEnergy(EnergyManager source)
    {
        if (_overHealRoutine == null)
        {
            _overHealRoutine = StartCoroutine(HealFromExcessEnergy(source));
        }
    }

    IEnumerator HealFromExcessEnergy(EnergyManager source)
    {
        int energyTakenWithoutHealing = 0;
        AddHealth(1);
        while(source.ExcessEnergy > 0.0f && Health < MaxHealth)
        {
            if (energyTakenWithoutHealing >= ExcessEnergyHealCost)
            {
                AddHealth(1);
                energyTakenWithoutHealing = 0;
            }
            source.AddEnergy(-1);
            energyTakenWithoutHealing++;

            yield return new WaitForSeconds(ExcessEnergyHealRate / ExcessEnergyHealCost);
        }

        _overHealRoutine = null;
    }
}
