using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArmFollow : MonoBehaviour {

	// Use this for initialization
	private Transform thisTransform;
	

	private Transform camTransform;

    [Header("References")]
    [SerializeField]
    private GameObject padRoot;
    private GameObject padStressMenu;

    [SerializeField]
    private GameObject watchRoot;
    private GameObject watchMenu;
   
   // [SerializeField]
	//private GameObject pad;
    
   
    [SerializeField] private Animator armAnimator;

    private Vector3 offset;
	private string curSceneName;

	//last item in hierarchy (tablet case "Cube")
    

	[SerializeField]private BoolVariable isStressMenuDisplayed;

	void Awake(){
		
		thisTransform = transform;
        padStressMenu = padRoot.transform.GetChild(0).gameObject;
        watchMenu = watchRoot.transform.GetChild(0).gameObject;

	}
	IEnumerator Start(){
		isStressMenuDisplayed.SetValue (false);

		camTransform = Camera.main.transform;
		//armAnimator = GetComponentInChildren<Animator> (());
		offset = thisTransform.position - Camera.main.transform.position;

  //      if(padStressMenu == null)
		//padStressMenu = GameObject.FindWithTag ("StressMenu");
  //      if(watchMenu == null)
		//watchMenu = GameObject.FindWithTag ("DirectMenu");

		if(SceneController.Instance != null)
		curSceneName = SceneController.Instance.GetCurrentSceneName ();

      //  if(pad == null)
		//pad = thisTransform.FindDeepChild ("Cube").gameObject;
		//if use close Menu makes closing sound in the beginign.


		CloseMenu (false);
		yield return null;

	

	}


	Quaternion rotation;

	bool isInitialLook = false;

	//this is checked off by button look click (BackToGame Button)
	bool isRemainMenuClosed = false;

	

	



	Vector3 oldViewingAngle;
	Vector3 curViewingAngle;

	bool isLerping = false;

	bool isButtonAvailable = true;


	private bool hasMenu;

    [SerializeField] private GameObject upgradeDisplay;
    [SerializeField] private GameObject stressMenuDisplay;
    public void OpenMenuTermporary(float time)
    {
        StartCoroutine(OpenMenuTemp(time));
        //stressMenu.SetActive(false);

    }
    public IEnumerator OpenMenuTemp(float time)
    {
        GameController.Instance.Paused = true;
        RemoveWatch();
        StartCoroutine(TriggerMenuWait());
        upgradeDisplay.SetActive(true);
        padStressMenu.SetActive(false);


        watchRoot.SetActive(false);
        stressMenuDisplay.SetActive(false);
        yield return new WaitForSecondsRealtime(time);
        stressMenuDisplay.SetActive(true);
        RemoveWatch();


        upgradeDisplay.SetActive(false);
        padStressMenu.SetActive(true);
        CloseMenu();

        //yield return new WaitForSecondsRealtime(1);
        //watchRoot.SetActive(true);

    }

    public void TriggerMenu(){
	//new
		GameController.Instance.Paused = true;
		RemoveWatch ();
		StartCoroutine (TriggerMenuWait ());

	}
	IEnumerator TriggerMenuWait(){



		while (!armAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
			yield return null;
		


		if( isButtonAvailable) {
			isButtonAvailable = false;

			AudioManager.Instance.PlayDirectSound ("TakeOutMenu");

			if (isInitialLook) {

				armAnimator.SetBool ("Close", false);
				yield break;
			}

			curViewingAngle = camTransform.rotation * Vector3.forward;

			thisTransform.GetChild(0).gameObject.SetActive(true);
			watchRoot.SetActive (false);

			padRoot.SetActive(true);

			oldViewingAngle =  Quaternion.Euler(0,90,0) * camTransform.forward ;
			isInitialLook = true;
			hasMenu = true;
			

		}


	}
    public void CloseMenu(bool hasSound = true)
    {


        watchRoot.SetActive(true);
        RemoveWatch();
        
        isStressMenuDisplayed.SetValue(false);


        if (hasSound)
            AudioManager.Instance.PlayDirectSound("TakeOutMenu");

        if (armAnimator.isActiveAndEnabled)
            armAnimator.SetBool("Close", true);

        GameController.Instance.Paused = false;
        padStressMenu.SetActive(false);
        isRemainMenuClosed = true;


        //new


    }
    public void TriggerWatch(){

		    StartCoroutine (TriggerWatch_Wait ());
			
			curViewingAngle = camTransform.rotation * Vector3.forward;

			thisTransform.GetChild(0).gameObject.SetActive(true);

			
		    watchMenu.SetActive (false);
			padRoot.SetActive(false);




			oldViewingAngle =  Quaternion.Euler(0,90,0) * camTransform.forward ;

			hasMenu = false;




	}
	IEnumerator TriggerWatch_Wait(){

		while (!armAnimator.isActiveAndEnabled)
			yield return null;

		armAnimator.SetBool ("Close", false);

		while (!armAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Idle"))
			yield return null;

		watchMenu.SetActive (true);

	}
    public void RemoveWatch()
    {

        if (armAnimator.isActiveAndEnabled)
            armAnimator.SetBool("Close", true);

        watchMenu.SetActive(false);
        isRemainMenuClosed = true;

    }


    void LateUpdate () {


		if (!armAnimator.gameObject.activeInHierarchy)
			return;


		if (isInitialLook) {


			if (armAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Idle")) {

				if (hasMenu) {
					isStressMenuDisplayed.SetValue (true);
					padStressMenu.SetActive (true);

				}


				isInitialLook = false;

				//ALLOW FOR INITIAL SET UP OF MENU TO PLAYERS FACE
				isLerping = true;
			
			
			}
		
		} else if (isRemainMenuClosed) {

		
			if (armAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Start")) {

				isRemainMenuClosed = false;

				armAnimator.SetBool ("Close", false);

				thisTransform.GetChild(0).gameObject.SetActive(false);


				isButtonAvailable = true;
				//new
				TriggerWatch ();


				//

			}

		
		}

	//	if (GameController.Instance.IsMenuActive) {


			curViewingAngle = camTransform.forward;

			if (Vector3.Angle (oldViewingAngle, curViewingAngle) > 45 || isLerping) {

				isLerping = true;

				rotation = Quaternion.Euler (0, camTransform.eulerAngles.y, 0);

				thisTransform.position = Vector3.Lerp (thisTransform.position, camTransform.position - (rotation * (offset * -1)), Time.unscaledDeltaTime * 3f);

				thisTransform.LookAt (2 * thisTransform.position - camTransform.position, Vector3.up);
			


					if (Vector3.Distance(thisTransform.position, camTransform.position - (rotation * (offset * -1))) < 0.05f){
					isLerping = false;
					oldViewingAngle = curViewingAngle;
					}
			}
			//Debug.Log (Vector3.Distance(thisTransform.position, camTransform.position - (rotation * (offset * -1))));
	//	}
			
		
	}
		
}
