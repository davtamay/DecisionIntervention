using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Parent_Event : MonoBehaviour
{
    public Transform parentTransform;
    public Transform childTransform;
    [Header("Event")]
    public UnityEvent onParented;
    [Header("Parenting Settings")]
    public bool keepWorldPosition = true;
    public bool setScaleToOne = true;

    /// <summary>
    /// Prevent From Invoking during Editor Timeline
    /// </summary>
    private bool canInvoke = false;

    public void Awake()
    {
        canInvoke = true;
    }
    public void SetParent()
    {

        if (!canInvoke)
            return;

        childTransform.SetParent(parentTransform, keepWorldPosition ? true : false);

        onParented.Invoke();

        if (setScaleToOne)
            childTransform.localScale = Vector3.one;

        
    }



}
