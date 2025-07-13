using Services;
using UnityEngine;

public class EnergyWallet : IService
{
    public int Energy {get; private set;}

    public EnergyWallet(int energy)
    {
        Energy = energy;
    }

    private bool IsValueValid(int value)
    {
        if (value < 0 || value > Energy)
        {
            Debug.Log($"Value {value} is out of range.");
            return false;
        }
        
        return true;
    }
    
    public bool IsSpendSuccessful(int value)
    {
        if (!IsValueValid(value))
            return false;
        
        Energy -= value;
        EventBus.SendOnEnergyChanged(Energy);
        Debug.Log($"Energy on wallet {Energy}");
        return true;
    }

    public bool IsGetEnergy(int value)
    {
        if (value < 0)
            return false;
        
        Energy += value;
        EventBus.SendOnEnergyChanged(Energy);
        return true;
    }
}