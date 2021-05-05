using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableCall : MonoBehaviour
{
    
    public UnityEvent onEnableTrigger;
    public float seconds = 1f;

    public void OnEnable()
    {
        Invoke("Trigger", seconds);
       // onEnableTrigger.Invoke();
    }
    public void Trigger()
    {
        onEnableTrigger.Invoke();
    }
}
