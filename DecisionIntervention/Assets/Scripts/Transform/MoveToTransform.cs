using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTransform : MonoBehaviour {


    private Transform thisTransform;
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 eulerAngle;
    [SerializeField] Vector3 localScale;

    public void Awake()
    {
        thisTransform = transform;
    }
    public void MoveToPosition()
    {
        thisTransform.localPosition = this.position;
    }
    public void MoveToRotation()
    {
        thisTransform.eulerAngles = this.eulerAngle;
    }
    public void MoveToPositionAndRotation()
    {
        thisTransform.localPosition = this.position;
        thisTransform.eulerAngles = this.eulerAngle;

    }
    public void MovetoLocalScale()
    {
        thisTransform.localScale = localScale;

    }
}
