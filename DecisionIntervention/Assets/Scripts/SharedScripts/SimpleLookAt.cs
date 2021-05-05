using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLookAt : MonoBehaviour {

	[SerializeField] private Transform lookingTransform;
    [SerializeField] private Vector3 lookingModTransform;
	private Transform thisTransform;

    [SerializeField] private bool inverse;

	[SerializeField] private bool isEyeBall;

    [Header("Freeze Player Look At Direction")]
    [SerializeField] private bool isFreezeDirectionX = false;
    [SerializeField] private bool isFreezeDirectionY, isFreezeDirectionZ = false;

    //[SerializeField] private bool isIgnoreXRoation = false;

    void Awake(){
		thisTransform = transform;
	}
	public void LateUpdate () {

        //if (isIgnoreXRoation)
        //{
        //    lookingModTransform = lookingTransform.position;
        //    lookingModTransform.y = 90;
        //}else
            lookingModTransform = lookingTransform.position;

        if (isFreezeDirectionX)
            lookingModTransform.x = thisTransform.position.x;

        if (isFreezeDirectionY)
            lookingModTransform.y = thisTransform.position.y;

        if (isFreezeDirectionZ)
            lookingModTransform.z = thisTransform.position.z;
            



        if (isEyeBall)
        {
            Debug.DrawRay(lookingModTransform, -thisTransform.right);
            //thisTransform.LookAt (2 * (thisTransform.position - lookingTransform.position) -thisTransform.right, thisTransform.TransformDirection(Vector3.up));
            //Quaternion rotate =	Quaternion.FromToRotation(-thisTransform.right,(2 * (thisTransform.position - lookingTransform.position)));
            //Quaternion.AngleAxis(Vector3.SignedAngle(thisTransform
            //	thisTransform.rotation = rotate
            thisTransform.LookAt(lookingModTransform, thisTransform.TransformDirection(Vector3.forward));
            //	thisTransform.LookAt (2 * (thisTransform.position - lookingTransform.position));
        }
        else
        {
            if (inverse)
            {
                thisTransform.LookAt(2 * thisTransform.position - lookingModTransform, Vector3.up);
            }
            else
            {
                thisTransform.LookAt(lookingModTransform, Vector3.up);
            }
        }
			
	}

}
