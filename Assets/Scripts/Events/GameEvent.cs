using System;
using System.Collections.Generic;
using UnityEngine;


public class GameEvent<T> : ScriptableObject
{
    private List<Action<T>> listeners = new();

    public void Raise(T value)
    {
        if (listeners == null) return;

        foreach(Action<T> listener in listeners)
        {
            listener?.Invoke(value);
        }
        /*for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i]?.Invoke(value);
        }*/            
    }

    public void Register(Action<T> listener)
    {
        listeners.Add(listener);
    }

    public void Unregister(Action<T> listener)
    {
        listeners?.Remove(listener);
    }

}
