using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateSubject : MonoBehaviour
{
    private List<IObserver> _observers = new List<IObserver>();

    //Add observer to subject's collection
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }
    
    //Remove observer from subject's collection
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
    
    //Notify each observer that an event has occurred
    protected void NotifyObservers()
    {
        _observers.ForEach((_observer) =>
        {
            _observer.OnNotify();
        });
    }
}
