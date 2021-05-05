using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park_LonelyDog_TaskCollection : CollectTaskInteraction {

	[Header("Movement For Head Settings")]
	[SerializeField] Transform headBone;
	[SerializeField]float weight = 1f;
	[SerializeField]float dampTime;
	[SerializeField]float maxAngle;
	[SerializeField]Vector3 additionalRotation;
	[SerializeField] bool isLooking = false;
	[SerializeField] Transform target;
	private Animator thisAnimator;


	[Header("Walking Settings After Completion")]
	[SerializeField] Transform wayPointsGO;
	[SerializeField] float speed = 10f;
	
	DogInteraction dogInteractionScript;

    [Header("Reference")]
    [SerializeField] Transform dogFollowing;
    //[SerializeField] bool isCompleted;
    private Task_Evaluation taskEvaluation;
    
    // public Task 



    public override void Awake ()
	{
		thisAnimator = GetComponent<Animator> ();

		base.Awake();

		dogInteractionScript = dogFollowing.GetComponent<DogInteraction> ();

		target = player;


	}

	public override void Start(){

        taskEvaluation = GetComponent<Task_Evaluation>();

		if (taskEvaluation.IsTaskCompleted()) {

		Collider[] cols = thisTransform.GetComponents<Collider> ();

		foreach (Collider col in cols)
			if (col.isTrigger)
				Destroy(col);
			//	col.enabled = false;

		cols = dogFollowing.GetComponents<Collider> ();

		foreach (Collider col in cols)
			if (col.isTrigger)
			//	col.enabled = false;
				Destroy(col);

			onCompletion.Invoke ();
			dogInteractionScript.onCompletion.Invoke ();
			
		
			target = dogFollowing;
			dogFollowing.position = new Vector3 (thisTransform.position.x, dogFollowing.position.y, thisTransform.position.z);
			dogInteractionScript.target = thisTransform.GetChild (0);
			StartCoroutine (dogInteractionScript.Follow ());

			dogFollowing.GetComponent<RandomMoveAnimations> ().isRandomOn = false;

			thisAnimator.SetTrigger ("GetUp");
			thisAnimator.SetBool ("IsWalking", true);

			StartCoroutine (TakeWalk ());
		}
	}

	public void OnSpeak(){

        switch (taskEvaluation.GetCurrentTaskEvaluationState())
        {
            case Task_Status.NOT_IDENTIFIED:
                taskEvaluation.SaveTaskIdentified();
                break;
            case Task_Status.IDENTIFIED:

                if (dogInteractionScript.isFollowing)
                    taskEvaluation.SaveTaskCompletion();

                break;
            case Task_Status.COMPLETED:
                break;
        }
       
		//if (!PlayerPrefs.HasKey (nameForPlayerPref)) {
			//SaveTaskIdentified ();
	//	}
//FIXME commented this out for localization implementation ---		infoTextComponent.text = infoText;

		//TriggerInfo ();
		//CheckForTaskCompletion ();
	}


	public override void CheckForTaskCompletion ()
	{

		if (DogInteraction.curDogInteraction != null) {

			SaveTaskCompletion ();

			Collider[] cols = thisTransform.GetComponents<Collider> ();

			foreach (Collider col in cols)
				if (col.isTrigger)
					//col.enabled = false;
					Destroy(col);
			
			cols = dogFollowing.GetComponents<Collider> ();

			foreach (Collider col in cols)
				if (col.isTrigger)
					//col.enabled = false;
					Destroy(col);

			onCompletion.Invoke ();
			dogInteractionScript.onCompletion.Invoke ();

			target = dogFollowing;
			dogFollowing.GetComponent<DogInteraction> ().target = thisTransform.GetChild (0);
			StartCoroutine (dogFollowing.GetComponent<DogInteraction> ().Follow ());

			dogFollowing.GetComponent<RandomMoveAnimations> ().isRandomOn = false;

			thisAnimator.SetTrigger ("GetUp");
			thisAnimator.SetBool ("IsWalking", true);

			StartCoroutine (TakeWalk ());
		}
	}
	public override void OnTriggerEnter(Collider other){

		if (other.CompareTag ("Player")) {
            
            //if(task.taskStatus == Task_Status.NOT_IDENTIFIED)
            //task.taskStatus = Task_Status.IDENTIFIED;
		//	DATA_MANAGER.SaveTaskStatus (task, Task_Status.IDENTIFIED);

			if (DogInteraction.curDogInteraction != null)
				target = DogInteraction.curDogInteraction.dogTransform;

			onTriggerEnter.Invoke ();

		}
	}

	public override void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			onTriggerExit.Invoke ();

		}

	}

	public override void OnTriggerStay(Collider other){

		return;
	}

	private Vector3 lookDirection;
	private Vector3 finalLookVector;
	private Quaternion rotation;

	void LateUpdate(){



		if (weight <= 0f)
			return;

		//Mathf.Lerp (weight, 0f, Time.deltaTime);

		Vector3 dampVelocity = Vector3.zero;

		lookDirection = Vector3.SmoothDamp (lookDirection, target.position - headBone.position, ref dampVelocity, dampTime);



		if (Vector3.Angle (lookDirection, transform.forward) > maxAngle) {
			finalLookVector = Vector3.RotateTowards (thisTransform.forward, lookDirection, Mathf.Deg2Rad * maxAngle, 0.5f);
			thisAnimator.SetBool ("IsLooking", false);
			//thisAnimator.CrossFade ("LookRaiseArm", Time.deltaTime );
			isLooking = false;
		} else {
			finalLookVector = lookDirection;
			thisAnimator.SetBool("IsLooking", true);
			isLooking = true;
		}

		if (!isLooking)
			return;

        if (finalLookVector != Vector3.zero || additionalRotation != Vector3.zero)
        {
            // Do the rotation here
            rotation = Quaternion.LookRotation(finalLookVector) * Quaternion.Euler(additionalRotation);
            headBone.rotation = Quaternion.Lerp(headBone.rotation, rotation, weight);
        }


	}


	IEnumerator TakeWalk(){
	
		while (thisAnimator.GetCurrentAnimatorStateInfo (0).IsName ("GetUp"))
			yield return null;

		thisAnimator.applyRootMotion = true;

		Vector3[] wayPoints = new Vector3[wayPointsGO.childCount]; 

		for(int i = 0; i < wayPoints.Length; i++){
		//foreach(Transform wP in wayPointsGO.transform){
			wayPoints[i] = wayPointsGO.GetChild(i).position;

			wayPoints [i].y = thisTransform.position.y;


		}

		Vector3 direction = Vector3.zero;
		int e = 0;

		while (true) {


			if (Vector3.Distance (thisTransform.position, wayPoints [e]) < 5) {
				++e;

				if (e >= wayPoints.Length)
					e = 0;



				//thisTransform.rotation = Quaternion.RotateTowards (thisTransform.rotation, Quaternion.LookRotation (direction), 1f);


			}

			direction = Vector3.Normalize(wayPoints [e] - thisTransform.position);

			thisTransform.rotation = Quaternion.RotateTowards (thisTransform.rotation, Quaternion.LookRotation (direction), 4f);
			thisTransform.position += direction * Time.deltaTime * speed; 
		

			yield return null;


		
		
		}
	
	
	}
}
