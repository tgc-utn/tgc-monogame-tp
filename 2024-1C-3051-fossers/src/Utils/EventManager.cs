using System;
using System.Collections.Generic;

public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance => _instance ?? (_instance = new EventManager());

    private Dictionary<string, Action<object>> _eventDictionary;

    private EventManager()
    {
        _eventDictionary = new Dictionary<string, Action<object>>();
    }

    public void StartListening(string eventName, Action<object> listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent += listener;
            _eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent = new Action<object>(listener);
            _eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(string eventName, Action<object> listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= listener;
            _eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(string eventName, object parameter = null)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(parameter);
        }
    }
}