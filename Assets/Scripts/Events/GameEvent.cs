using System;
using System.Collections.Generic;
using UnityEngine;


public class GameEvent<T> : ScriptableObject
{
    private List<Action<T>> listeners = new();

    public void Raise(T value)
    {
        if (listeners == null) return;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i]?.Invoke(value);
        }            
    }

    public void Register(Action<T> listener)
    {
        listeners.Add(listener);
    }

    public void Register(Action<T>[] newListeners)
    {
        foreach(Action<T> listener in newListeners)
        {
            listeners.Add(listener);
        }        
    }

    public void Unregister(Action<T> listener)
    {
        listeners?.Remove(listener);
    }

    public void Unregister(Action<T>[] newListeners)
    {
        foreach(Action<T> listener in newListeners)
        {
            listeners?.Remove(listener);
        }        
    }
}
