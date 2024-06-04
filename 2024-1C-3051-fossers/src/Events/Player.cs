using System;

public static class PlayerEvents
{

    public static void TriggerReload(int reloadingTime)
    {
        EventManager.Instance.TriggerEvent(Constants.Events.Player.RELOADING, reloadingTime);
    }

    public static void SubscribeToReload(Action<int> cb)
    {
        EventManager.Instance.StartListening(Constants.Events.Player.RELOADING, cb);
    }

    public static void TriggerHealthChanged(int newHealthValue)
    {
        EventManager.Instance.TriggerEvent(Constants.Events.Player.RELOADING, newHealthValue);
    }

    public static void SubscribeToHealthChanged(Action<int> cb)
    {
        EventManager.Instance.StartListening(Constants.Events.Player.HEALTH_CHANGED, cb);
    }
}