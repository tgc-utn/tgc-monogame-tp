using System;
using System.Collections.Generic;

public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance => _instance ?? (_instance = new EventManager());

    private Dictionary<string, List<Delegate>> _eventDictionary;

    private EventManager()
    {
        _eventDictionary = new Dictionary<string, List<Delegate>>();
    }

    public void StartListening<T>(string eventName, Action<T> listener)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = new List<Delegate>();
        }

        _eventDictionary[eventName].Add(listener);

        if (Constants.DEBUG_MODE)
        {
            Console.WriteLine($"Listener added for event: {eventName}");
        }
    }

    public void TriggerEvent<T>(string eventName, T eventArgs)
    {
        if (_eventDictionary.TryGetValue(eventName, out var listeners))
        {
            foreach (Delegate listener in listeners)
            {
                var action = listener as Action<T>;
                action?.Invoke(eventArgs);
            }

            if (Constants.DEBUG_MODE)
            {
                Console.WriteLine($"Listener removed for event: {eventName}");
            }
        }

        // Check for debug mode and log the event if true
        if (Constants.DEBUG_MODE)
        {
            Console.WriteLine($"Event triggered: {eventName}");
        }
    }
}