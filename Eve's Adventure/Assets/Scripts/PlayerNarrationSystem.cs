using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNarrationSystem : MonoBehaviour, IObserver
{
    [SerializeField] PlayerStateSubject _playerSubject;
    public void OnNotify()
    { 
        Debug.Log("NARRATION ACTIVATED! HERE, HAVE SOME STORY YOU DIDN'T ASK FOR!");
    }

    //called when gameobject is enabled
    private void OnEnable()
    {
        //add itself to the subject's list of observers
        _playerSubject.AddObserver(this);
    }
    
    //called when gameobject is disabled
    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }
}
