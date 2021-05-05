using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
//using RoboRyanTron.Unite2017.Events;

public class LookSelect : MonoBehaviour {

	//need to be provided a GO for it to work in VR
	public static bool isTimerAFactor = true;

	public GameObject selectedCard;
	public GameObject notNullOnSelected;
	public Camera cam;
	public GameObject firstCard;
	public GameObject secondCard;
	public bool isFirstCard;
	public bool isSecondCard;

	public int setCompleted;
	public ParticleSystem particles;
	public int spawnedCards;
	private CardSpawner cardSpawner;
	//public Text coinText;
	private LookSelect lookSelect;

    //[SerializeField] GameEvent onAllCardsReveiledEvent;
    [SerializeField] UnityEvent onAllCardsReveiled;
	//[SerializeField] float timeToBeatFirstLevel;
	//[SerializeField] float timeToBeatSecondLevel;
	////Beating second levels accomplishes mission, 3rd level is to see highscore?
	//[SerializeField] float timeToBeatThirdLevel;

	[SerializeField] Animator opponentAnimator;

    [Header("References")]

    [SerializeField] private IntVariable currentCardsAvailable;
    [SerializeField] private Points highScorePointsToCheck;
	[SerializeField]private DataManager DATA_MANAGER;
    //[SerializeField]private 

    [Header("Debug_Action")]
    [SerializeField] private bool isChangeRound = false;


	void Start (){

		selectedCard = notNullOnSelected;

		//cardSpawner = GetComponent<CardSpawner> ();
		cam = Camera.main;

        spawnedCards = currentCardsAvailable.Value;//cardSpawner.GetSpawned;

       // WaveManager.Instance.TimeToAdd(ref isTimerDone, 1f);



        //WaveManager.Instance.StopTimer ();
        //WaveManager.Instance.TimeToAdd(ref isTimerDone, timeToBeatFirstLevel);
        //GameController.Instance.TimeToAdd (ref isTimerDone, timeToBeatFirstLevel);
    }

//	void OnEvent(EVENT_TYPE Event_Type, Component Sender, params object[] Param){
//
//		switch(Event_Type){
//
//		case EVENT_TYPE.GAME_PAUSED:
//
//			WaveManager.Instance.StopTimer ();
//			break;
//
//		case EVENT_TYPE.GAME_UNPAUSED:
//
//			WaveManager.Instance.ResumeTimer ();
//			break;
//
//
//		}
//
//	}

	bool isTimerDone = false;
	void Update(){

		
  //      if (isTimerAFactor) {
		
		//	WaveManager.Instance.TimeToAdd (ref isTimerDone);
		


		//}

		//if (isTimerDone) {

  //          if (DATA_MANAGER.masterPlayerPoints.currentPlayerPoints.Value > highScorePointsToCheck.Value)
  //              highScorePointsToCheck = DATA_MANAGER.masterPlayerPoints.currentPlayerPoints;

         
  //          GameController.Instance.isGameOver = true;
  //          this.enabled = false;


            //GameController.Instance.Paused = true;
            //this.enabled = false;

    //    }


        

		Ray ray = new Ray (cam.transform.position, cam.transform.rotation * Vector3.forward);

		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 120)) {
			
				if (hit.transform.gameObject.CompareTag ("Card")) {

					if (hit.transform.gameObject.GetInstanceID () == selectedCard.GetInstanceID () || isSecondCard)
						return;
				

					selectedCard = hit.transform.gameObject;

				
				
					StartCoroutine ("CardSelect");
				}

			
		}
   }
	IEnumerator CardSelect(){

		if (!isFirstCard && !isSecondCard) {
			
			isFirstCard = true;
			firstCard = selectedCard;

			AudioManager.Instance.PlayInterfaceSound ("CardSelect");
			StartCoroutine (RotateCard (firstCard));
		
			yield break;
		
		}else if (isFirstCard && !isSecondCard){
			
			isFirstCard = false;
			isSecondCard = true;

			secondCard = selectedCard;

			AudioManager.Instance.PlayInterfaceSound ("CardSelect");
			StartCoroutine (TestCards (firstCard, secondCard));
			StartCoroutine (RotateCard (secondCard));



		

		}

	
	}

	IEnumerator RotateCard (GameObject selected){

		Vector3 rot = new Vector3 (0,180,0);
		bool isRotating = true;
		float timeToRotate = 0.0f;

		Vector3 selectedCurRot = selected.transform.localEulerAngles;

		while (isRotating) {

			yield return null;

			timeToRotate += Time.deltaTime;

			//selected.transform.rotation = Quaternion.LE (selected.transform.rotation, selected.transform.rotation * Quaternion.AngleAxis (180, Vector3.up), Time.deltaTime * 2f);
				//Quaternion.AngleAxis (180, Vector3.up); //Quaternion.Lerp (selected.transform.rotation, selected.transform.rotation * Quaternion.AngleAxis (180, Vector3.up), Time.deltaTime * 2f);
				//Quaternion.RotateTowards (selected.transform.rotation, selected.transform.rotation * Quaternion.AngleAxis(90, Vector3.up), 5f);
				// Quaternion.LookRotation (-selected.transform.forward, Vector3.up);
				;//Quaternion.RotateTowards (selected.transform.rotation, Quaternion.AngleAxis(180, Vector3.up), 5f);
			selected.transform.Rotate (rot * Time.deltaTime);


			//error cards are not turning all the way
			if (timeToRotate > 1f){

		//	if (timeToRotate > 5f){
					isRotating = false;
					yield break;
			
			}
		}
	
	}
    [SerializeField] private int currentWave;
	IEnumerator TestCards(GameObject a, GameObject b){
		
		yield return new WaitForSeconds (1.4f);
		if (string.Equals(a.name,b.name, System.StringComparison.CurrentCultureIgnoreCase) || isChangeRound)
		{
			a.GetComponent<MeshCollider> ().enabled = false;
			b.GetComponent<MeshCollider> ().enabled = false;
			
			PlayerManager.Instance.points = 1;

			Debug.Log ("we have a match!");

            //AudioManager.Instance.PlayDirectSound ("SmallWin");
            currentCardsAvailable.Value -= 2;
		//	spawnedCards -= 2;

		//	PlayerManager.Instance.points = 1;


			if (currentCardsAvailable.Value == 0 || isChangeRound) {

                firstCard = null;
                secondCard = null;
               
                
                currentCardsAvailable.Value = 0;
                onAllCardsReveiled.Invoke();

                //WaveManager.Instance.StopTimer();
                
                //WaveManager.Instance.ChangeTONextWaveAndAddTime();
                
				opponentAnimator.SetTrigger ("Fall1");
			//	WaveManager.Instance.StopTimer ();
			
		//		WaveManager.Instance.ChangeTONextWaveAndAddTime ();

				particles.Play ();
				yield return new WaitForSeconds (3);
				particles.Stop();
			
				if (currentWave == 0){//cardSpawner.GetWave == 0) {

					//AudioManager.Instance.PlayInterfaceSound ("MediumWin");

					//cardSpawner.ChangeWave (Difficulty.medium);
				//	WaveManager.Instance.StopTimer ();
					//WaveManager.Instance.ResetTimer ();
					//WaveManager.Instance.TimeToAdd(ref isTimerDone)
					//GameController.Instance.TimeToAdd (ref isTimerDone, timeToBeatSecondLevel);

					//spawnedCards = cardSpawner.GetSpawned;

                    

				} else if (currentWave == 1)
                {//(cardSpawner.GetWave == 1) {

					//AudioManager.Instance.PlayInterfaceSound ("MediumWin");

				//	cardSpawner.ChangeWave (Difficulty.hard);
				//	GameController.Instance.TimeToAdd (ref isTimerDone, timeToBeatThirdLevel);
				//	spawnedCards = cardSpawner.GetSpawned;
				

				}else if (currentWave == 2){//(cardSpawner.GetWave == 2) {
                    opponentAnimator.SetTrigger ("Fall2");
                   // AudioManager.Instance.PlayInterfaceSound("BigWin");
					GameController.Instance.isGameOver = true;

				}
               
                currentWave += 1;

                //WaveManager.Instance.ResumeTimer ();
                
				//GameController.Instance.ResumeTimer ();
			}

		}else {
			

			yield return StartCoroutine (RotateCard (firstCard));
			//85 is the original rotation of the card may have to change this by keeping and reflecting original value
			firstCard.transform.localEulerAngles = new Vector3 (85, 0,0); //Vector3.zero;
			firstCard = null;

		
	
		//	yield return *error: turn back faster
			yield return StartCoroutine (RotateCard (secondCard));

			secondCard.transform.localEulerAngles = new Vector3 (85, 0,0);//Vector3.zero;
			secondCard = null;

		
			selectedCard = notNullOnSelected;



		}
		isSecondCard = false;
	}


}
