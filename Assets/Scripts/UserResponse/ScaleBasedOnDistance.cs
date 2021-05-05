using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBasedOnDistance : MonoBehaviour {

    float lookDistance;
    Transform cam;
    Transform thisTransform;

    [SerializeField] float minSize = 0.1f;
    [SerializeField] float maxSize = 1f;

    [SerializeField] float defaultSize = 0.2f;
	// Use this for initialization
	void Start () {

        cam = Camera.main.transform;
        thisTransform = transform;
		
	}
	
	// Update is called once per frame
	void Update () {
        if(cam == null)
            cam = Camera.main.transform;
        lookDistance = Vector3.Magnitude(thisTransform.transform.position - cam.transform.position);


        var distanceToSize = Vector3.one * (lookDistance * 0.2f);
       // if(defaultSize < thisTransform.localScale.x)
        thisTransform.localScale = new Vector3( Mathf.Clamp( distanceToSize.x, minSize,maxSize), Mathf.Clamp(distanceToSize.y, minSize, maxSize),1) ;
    }
}
