using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using TagFrenzy;
[Serializable]
public class Vector3_UnityEvent : UnityEvent<Vector3>{}

public class ButtonClickLook : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler {
	//currently only have meditation on this event;
	private Vector3_UnityEvent vector3_UnityEvent;
	private Vector3 vector3_UnityEvent_Parameter;

	[SerializeField]private UnityEvent OnClick;

    //[Tooltip("Call Button Component OnClick Event")]
	//[SerializeField] private bool isOnClickEventCalled = false;

	public Image buttonFill;
	//private Button button;


	[SerializeField] private bool isStressModified;
	[SerializeField] private float stressModifiedAmount;

	private float secondsUntilClick;

	[SerializeField]private bool isCustomTime;
	[SerializeField]private float customTime;

	[SerializeField]private float originalTime;

	[Header("References")]
	[SerializeField]private BoolVariable isSceneLoading;


	void Awake(){
	
		buttonFill = GetComponent <Image> ();
		buttonFill.fillAmount = 1.0f;

		secondsUntilClick = GazeInputModule.GazeTimeInSeconds;
			
		//button = GetComponent <Button> ();
	}
	//public void Start(){
	//	//vector3_UnityEvent.AddListener (InvokeVector3UnityEvent);

	//}
	public void InvokeVector3UnityEvent(Vector3 value){

	
		vector3_UnityEvent.Invoke (vector3_UnityEvent_Parameter);

	}

	public void OnPointerEnter(PointerEventData eventData){

        if (!EventSystem.current.IsPointerOverGameObject ()) {
			//block back objects from being clicked https://www.youtube.com/watch?v=EVZiv7DLU6E

			//Debug.Log ("I AM ONNNNNBUTTON!!!!!!");


			if (isCustomTime) {
				originalTime = GazeInputModule.GazeTimeInSeconds;
				GazeInputModule.GazeTimeInSeconds = customTime;
			}

	
			StartCoroutine (ReduceFillAmount (eventData));

		}
	}
	IEnumerator ReduceFillAmount(PointerEventData eventData){

		if (isSceneLoading.isOn) {
			EventSystem.current.SetSelectedGameObject (null);
			//StopAllCoroutines ();
			//yield break;
		}
			
		secondsUntilClick = GazeInputModule.GazeTimeInSeconds;
		float totalTimeToWait = secondsUntilClick;

		while(true){
			
			secondsUntilClick -= Time.unscaledDeltaTime;
			buttonFill.fillAmount = secondsUntilClick/totalTimeToWait;
			yield return null;

		}

	}
	public void OnPointerExit(PointerEventData eventData){

		StopAllCoroutines ();
		buttonFill.fillAmount = 1;

		if (isCustomTime) 
			GazeInputModule.GazeTimeInSeconds = originalTime;

		Debug.Log("I AM Out!!!!!!");

	}
	public void OnPointerClick(PointerEventData eventData){
		StopAllCoroutines ();
		buttonFill.fillAmount = 1;

		Debug.Log("I AM clickeed!!!!!!");

		if (isCustomTime) {
			GazeInputModule.GazeTimeInSeconds = originalTime;
		}
//
//		else if (isEnvChanger)
//			SceneController.Instance.ChangeSkyBox ();
//		else if (isBackToIntroButton) {
//
//			DATA_MANAGER.CheckHighScore ();
//		
//			LoadScene ("Intro");
//
//		}
//		else if (isStartButton) {
//			GameController.Instance.StartGame ();
//			AUDIO_MANAGER.PlayInterfaceSound ("SpecialSelect");
//			GameController.Instance.Paused = false;
//
//		} else if (isReplayButton) {
//			SceneController.Instance.ResetCurrentGame ();
//		}
//
//		if (isSceneChange)
//			LoadScene (scene);
//		
	//	if (isOnClickEventCalled)
        //    InvokeVector3UnityEvent();
        OnClick.Invoke ();
            

	}
		
//	void LoadScene(string scene){
//
//		SceneController.Instance.Load (scene);
//	
//	}
	public void LoadURL(string url){

		//if (isStressModified)
		//	UIStressGage.Instance.stress = stressModifiedAmount;
		
		Application.OpenURL (url);
		Application.Quit ();
	

	}
		
			
	}


	

	




