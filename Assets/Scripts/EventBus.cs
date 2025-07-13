using System;

public static class EventBus
{
    public static event Action<int> OnEnergyChanged;
    
    public static event Action OnActivityOpen;
    public static event Action OnActivityClose;

    public static event Action OnPlayerFinishedPath;

    public static void SendOnEnergyChanged(int energy)
    {
        OnEnergyChanged?.Invoke(energy);
    }

    public static void SendOnActivityOpen()
    {
        OnActivityOpen?.Invoke();
    }

    public static void SendOnActivityClose()
    {
        OnActivityClose?.Invoke();
    }

    public static void SendOnPlayerFinishedPath()
    {
        OnPlayerFinishedPath?.Invoke();
    }
}