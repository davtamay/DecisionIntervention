using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTransform : MonoBehaviour {

	Transform thisTransform;
	[SerializeField] private float speed;

	[Header("Move With Sin Settings")]
	[SerializeField] private bool isMoveWithSin;
	[SerializeField] private Vector3 positionRange;


	[Header("Move With Inside Random Sphere Settings")]
	[SerializeField] private bool isMoveWithSphereRandom;
	[SerializeField] private float radiusMagnitude;
	[SerializeField] private float distanceUntilRandomChange;
	[SerializeField] private bool xConstraint;
	[SerializeField] private bool yConstraint;
	[SerializeField] private bool zConstraint;
    [Space]

	[Header("Position Settings")]
	[SerializeField] private bool isMovePosition;
	[SerializeField] Vector3 moveMagnitude;
    [SerializeField] bool isUseTransformsToFlipDirections;
    [SerializeField] float distanceUntilFlip = 3f;
    [SerializeField] float isflipGAME_OBJECTOrientation = 3f;

    //[SerializeField] float clampMin, clampMax;
    //[SerializeField] bool isInvertPosOnClampReach;
    //[SerializeField] private Vector3 invertedMoveMagnitude;
    //[SerializeField] private Vector3 nonInvertedMoveMagnitude;
    //[SerializeField] private float invertedClampMin, invertedClampMax;

    [SerializeField] private Transform beginTransform, endTransform;

    [Space]



    [Header("Rotate Settings")]
	[SerializeField] private bool isRotate;
	[SerializeField] Vector3 rotateMagnitude;

	private Vector3 originalPosition;
	private Vector3 curRandomPos;

    [Header("Random Time Settings")]
    [SerializeField] private bool isCheckTimeScale;
    [SerializeField] private bool isRandomizeTimeForMovement;
    [SerializeField] private int randomMagnitude;
    private bool isReachedRandom = false;


    
    void Start () {
		
		thisTransform = transform;
		originalPosition = thisTransform.position;

        //if (isInvertPosOnClampReach)
        //{

    
        //    nonInvertedMoveMagnitude = moveMagnitude;
        //    invertedMoveMagnitude = -1 * moveMagnitude;

        //}

    }

    [SerializeField]private bool isInverted;
  

    // Update is called once per frame
    private bool isFirstSearch = false;
	void Update () {

        if (isRandomizeTimeForMovement && !isReachedRandom )
        {
            if (isCheckTimeScale)
                if (Time.timeScale == 0)
                    return;

            if (Random.Range(-randomMagnitude, randomMagnitude) != 0)
                return;
            else
            {
                isRandomizeTimeForMovement = false;
                isReachedRandom = true;

            }

        }
		
		if(isMoveWithSin)
		thisTransform.position =  new Vector3 ( thisTransform.position.x + positionRange.x *  Mathf.Sin (Time.time * speed),thisTransform.position.y + positionRange.y *  Mathf.Sin (Time.time * speed), thisTransform.position.z + positionRange.z *  Mathf.Sin (Time.time * speed));

		if (isMoveWithSphereRandom) {



			if (Vector3.Distance (curRandomPos, thisTransform.position) < distanceUntilRandomChange || !isFirstSearch) {
			
				isFirstSearch = true;

				curRandomPos = originalPosition + Random.insideUnitSphere * radiusMagnitude;

				if (xConstraint)
					curRandomPos.x = thisTransform.position.x;
				if (yConstraint)
					curRandomPos.y = thisTransform.position.y;
				if (zConstraint)
					curRandomPos.z = thisTransform.position.z;
			

			}
		
			thisTransform.position += (curRandomPos - thisTransform.position).normalized * Time.deltaTime * speed ;
		
		}
		if (isRotate) 
			thisTransform.Rotate (rotateMagnitude * Time.deltaTime, Space.Self);

        if (isMovePosition)
        {
            if (isUseTransformsToFlipDirections)
            {
              //  var tempPos = thisTransform.position;//+ (moveMagnitude * Time.deltaTime);//originalPosition;


                
                    

                if (!isInverted)
                {
                    thisTransform.position = Vector3.MoveTowards(thisTransform.position, endTransform.position, speed * Time.deltaTime);
                    float dis = (thisTransform.position - endTransform.position).magnitude;

                    if (dis < distanceUntilFlip)
                    {
                        isInverted = true;
                        thisTransform.localRotation = Quaternion.AngleAxis(180, Vector3.left);
                        isRandomizeTimeForMovement = true;
                        isReachedRandom = false;//thisTransform.rotation. thisTransform.rotation.ToAngleAxis( 180f, Vector3.up);
                    }
                }
                else
                {
                    thisTransform.position = Vector3.MoveTowards(thisTransform.position, beginTransform.position, speed * Time.deltaTime);
                    
                    float dis = (thisTransform.position - beginTransform.position).magnitude;

                    if (dis < distanceUntilFlip)
                    {
                        isInverted = false;
                        thisTransform.localRotation = Quaternion.AngleAxis(180, Vector3.left);
                        isRandomizeTimeForMovement = true;
                        isReachedRandom = false;
                    }

                }
                

             
               //thisTransform.Translate((!isInverted ? endTransform.position - beginTransform.position : beginTransform.position - endTransform.position ) * (moveMagnitude.x *
               // Time.deltaTime), Space.Self);
                
            }
            else
                thisTransform.Translate(moveMagnitude * Time.deltaTime, Space.Self);

        }
			
	}

	void OnTriggerEnter(){

		isFirstSearch = false;
	}
}
