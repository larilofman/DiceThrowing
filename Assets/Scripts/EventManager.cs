using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public UnityEvent EventReadyToThrow;
    public UnityEvent EventThrowPressed;
    public UnityEvent EventSetupOpened;
    public UnityEvent EventSetupAccepted;
    public UnityEvent EventAllDiceStopped;

    public void Ready()
    {
        EventReadyToThrow.Invoke();
    }
    public void Throw()
    {
        EventThrowPressed.Invoke();
    }

    public void OpenSetup()
    {
        EventSetupOpened.Invoke();
    }

    public void AcceptSetup()
    {
        EventSetupAccepted.Invoke();
    }

    public void AllDiceStopped()
    {
        EventAllDiceStopped.Invoke();
    }
}
