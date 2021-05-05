using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableProject : MonoBehaviour {
    [Header("ProjectionAxis")]
    [SerializeField] private bool x, y, z;
    [SerializeField] private float magnitude;
    [SerializeField] private Transform targetTransform;
    private Transform thisTransform;

    public void Awake()
    {
        thisTransform = transform;

    }
    public void OnStartFollowing()
    {
        
        InvokeRepeating("Project",0.01f,0.01f);
    }
    public void OnStopFollowing()
    {
        CancelInvoke();

    }
    Vector3 lineStart;
    Vector3 lineEnd;
    Vector3 normal;
    //public void LateUpdate()
    //{
    //    Debug.Log("BARRIERISFOLLOWING");
    //    Project();
    //}
    public void Project()
    {
       // Debug.Log("BARRIERISFOLLOWING");

        // thisTransform = transform;
        if (x)
        {
            lineStart = thisTransform.position + new Vector3(magnitude, 0, 0);
            lineEnd = thisTransform.position + new Vector3(-magnitude, 0, 0);
        }
        if (y)
        {
            lineStart = thisTransform.position + new Vector3(0,magnitude, 0);
            lineEnd = thisTransform.position + new Vector3(0,-magnitude, 0);
        }
        if (z)
        {
            lineStart = thisTransform.position + new Vector3(0,0, magnitude);
            lineEnd = thisTransform.position + new Vector3(0, 0, -magnitude);
        }


        normal = (lineStart - lineEnd).normalized;

        Vector3 pos = lineStart + Vector3.Project(targetTransform.position - lineStart, normal);

        pos.x = Mathf.Clamp(pos.x, Mathf.Min(lineStart.x, lineEnd.x), Mathf.Max(lineStart.x, lineEnd.x));
        pos.y = Mathf.Clamp(pos.y, Mathf.Min(lineStart.y, lineEnd.y), Mathf.Max(lineStart.y, lineEnd.y));
        pos.z = Mathf.Clamp(pos.z, Mathf.Min(lineStart.z, lineEnd.z), Mathf.Max(lineStart.z, lineEnd.z));

        thisTransform.position = pos;
    }

    public void OnDrawGizmos()
    {
        if (x)
        {
            Gizmos.color = Color.red;
            Vector3 lineStart = transform.position + new Vector3(magnitude, 0, 0);
            Vector3 lineEnd = transform.position + new Vector3(-magnitude, 0, 0);
           
            Gizmos.DrawLine(lineStart, lineEnd);
        }
        if (y)
        {
            Gizmos.color = Color.green;
            Vector3 lineStart = transform.position + new Vector3(0,magnitude, 0);
            Vector3 lineEnd = transform.position + new Vector3(0,-magnitude, 0);

            Gizmos.DrawLine(lineStart, lineEnd);
        }
        if (z)
        {
            Gizmos.color = Color.blue;
            Vector3 lineStart = transform.position + new Vector3(0,0, magnitude);
            Vector3 lineEnd = transform.position + new Vector3(0,0, -magnitude);

            Gizmos.DrawLine(lineStart, lineEnd);
        }
    }

}
	
