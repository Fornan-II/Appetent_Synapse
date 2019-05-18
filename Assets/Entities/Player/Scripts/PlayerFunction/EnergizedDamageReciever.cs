using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergizedDamageReciever : DamageReciever
{
    protected Coroutine _overHealRoutine;

    public bool LetHeal = true;
    public int MinimumEnergyForHeal = 16;
    public float BaseHealCost = 1.0f;
    public float ExcessEnergyHealRate = 0.5f;
    public int ExcessEnergyHealCost = 3;

    public virtual void BaseHeal(EnergyManager source)
    {
        //Debug.Log("Energy effect");
        if(_health < MaxHealth && LetHeal && source.Energy >= MinimumEnergyForHeal)
        {
            AddHealth(1);
            source.DrainRate.SetModifier("baseHealCost", BaseHealCost);
        }
        else
        {
            source.DrainRate.RemoveModifier("baseHealCost");
        }
    }

    public virtual void OnProcessExcessEnergy(EnergyManager source)
    {
        //Debug.Log("Process extra...");
        if (_overHealRoutine == null && LetHeal)
        {
            _overHealRoutine = StartCoroutine(HealFromExcessEnergy(source));
        }
    }

    IEnumerator HealFromExcessEnergy(EnergyManager source)
    {
        //Debug.Log("heal routine start");
        float healInterval = ExcessEnergyHealRate / ExcessEnergyHealCost;
        //Debug.Log("Heal interval: " + healInterval);
        //Debug.Log("Excess: " + source.ExcessEnergy + " | Health: " + _health);

        AddHealth(1);
        while(source.ExcessEnergy > 0.0f && _health < MaxHealth && LetHeal)
        {
            //Debug.Log("heal tick");
            //Debug.Log("Speed healing");
            AddHealth(1);
            source.AddEnergy(-1);

            yield return new WaitForSeconds(healInterval);
        }

        //Debug.Log("end heal routine");
        _overHealRoutine = null;
    }
}
