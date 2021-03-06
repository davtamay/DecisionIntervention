using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTaskInteraction : InteractionBehaviour {


	//[SerializeField]protected Transform collectObjParent;
	//[TextArea(0,15)][SerializeField]protected string textAfterCompletion;



	[SerializeField]protected GameObject objectToGive;
	[SerializeField]protected string nameForPlayerPref;

	[Header("References")]
	public DataManager DATA_MANAGER;
	//[SerializeField]protected Task task;
	public virtual void Start(){


		if (PlayerPrefs.GetInt (nameForPlayerPref) == 1) {
			//collectObjParent.gameObject.SetActive (false);

//FIXME commented this out for localization implementation --- infoTextComponent.text = textAfterCompletion;
		}//else if( Playe		rPrefs.GetInt (nameForPlayerPref) == 0)
		//	PlayerPrefs.SetInt(nameForPlayerPref,0);
		//else if(PlayerPrefs.HasKey(!nameForPlayerPref)
			
	}

	public virtual void OnTriggerEnter(Collider other){
	
		if (other.CompareTag ("Player")) {

			if (PlayerPrefs.GetInt (nameForPlayerPref, 0) == 0) {
				CheckForTaskCompletion ();
			} else 
				return;
			
		}
		onTriggerEnter.Invoke ();

	}
	public virtual void OnTriggerExit(Collider other){

		//if (other.CompareTag ("Player")) {

		//	infoCanvasPrefab.SetActive (false);

		//}
		onTriggerExit.Invoke ();
	}

	public virtual void OnTriggerStay(Collider other){

	
		//if (other.CompareTag ("Player")) {
		//	infoCanvasPrefab.transform.LookAt (2 * thisTransform.position - player.position);
			
		//}
	
	}

	public virtual void CheckForTaskCompletion(){

		/*
		int itemsCollected = 0;
		int cCount = collectObjParent.childCount;

		foreach(GameObject gO in PlayerManager.Instance.playerItemSlotGOList){
			
			for(int i = 0; i < cCount; i++){
				
				if(string.Equals(gO.name, collectObjParent.GetChild(i).name ,System.StringComparison.CurrentCultureIgnoreCase))
					++itemsCollected;
					
				}
			}

	//	Debug.Log("PlayerSlotUSED:" + PlayerManager.Instance.playerSlotGOList.Count + " GO TO Collect: " + gOsToCollect.Count + " ItemsCollected:" + itemsCollected);
		if (cCount == itemsCollected) {
			for (int i = 0; i < cCount; i++) {
				PlayerManager.Instance.RemoveItemFromSlot (collectObjParent.GetChild(i).gameObject);
				infoTextComponent.text = textAfterCompletion;
				if(objectToGive != null)
				PlayerManager.Instance.AddItemToSlot (objectToGive);
			}
				
			collectObjParent.gameObject.SetActive (false);
			PlayerPrefs.SetInt(nameForPlayerPref,1);
			return;

		}else
			return;
	*/
	}
	public void SaveTaskIdentified(){

        if (PlayerPrefs.HasKey(nameForPlayerPref) == false)
        {
            PlayerPrefs.SetInt(nameForPlayerPref, 0);
            PlayerPrefs.Save();
            QuestAssess.Instance.OnUpdate();


        }
        //
    }

	public bool IsTaskIdentified(){
		if (PlayerPrefs.HasKey (nameForPlayerPref) == true)
			return true;

		return false;
	
	}
	public void SaveTaskCompletion(){

        PlayerPrefs.SetInt(nameForPlayerPref, 1);
        PlayerPrefs.Save();
        QuestAssess.Instance.OnUpdate();

    }
    public bool IsTaskCompleted(){
		if (PlayerPrefs.GetInt (nameForPlayerPref) == 1)
			return true;

		return false;

	}

}
