using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameEvent<T> : ScriptableObject
{
    private List<Action<T>> listeners = new();

    public void Raise(T value)
    {
        if (listeners == null) return;

        foreach(Action<T> listener in listeners.ToList())
        {
            listener.Invoke(value);
        }          
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
