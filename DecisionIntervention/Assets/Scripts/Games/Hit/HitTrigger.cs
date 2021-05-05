using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HitTrigger : MonoBehaviour {

	public int pointsForCollider;

	[SerializeField]private bool isGround;

	[SerializeField]private Color originalColor;
	[SerializeField]private Color triggeredChangeColor1;
	[SerializeField]private Color triggeredChangeColor2;
	[SerializeField]private Color triggeredChangeColor3;

    //helps avoid the same objecttriggeting points on same collider multiple times.
	public GameObject gameObjectThatTriggeredMe = null;

	[SerializeField]private HitTrigger[] hitTriggers;

	[SerializeField]private bool isAllowPersist = true;

	void Start(){

        //originalColor = transform.GetComponent<MeshRenderer> ().material.color;
        if (isGround)
            hitTriggers = GetComponentsInChildren<HitTrigger>();
        //else
        //    hitTriggers[0] = GetComponent<HitTrigger>();


    }
	void OnTriggerEnter(Collider other){

		if (isGround) {
		//	if(HitManager.Instance.gONotInGround.Contains (other.gameObject))
		//	HitManager.Instance.gONotInGround.Remove (other.gameObject);
			//foreach(Transform t in transform)
			if (!other.CompareTag ("Obstacle"))
				return;


			other.GetComponent<MeshRenderer> ().material.color = originalColor;

            //new 11/9/2018
           // if (hitTriggers.Length < 1)
             //   return;
             
            if(hitTriggers.Length > 0)
			foreach (HitTrigger h in hitTriggers) {

                    if (h.gameObjectThatTriggeredMe == null)
                        continue;
				
				if (h.gameObjectThatTriggeredMe.GetInstanceID() == other.gameObject.GetInstanceID()) {
					
					h.gameObjectThatTriggeredMe = null;

				}
			}
				

		}

		if (gameObjectThatTriggeredMe == null || gameObjectThatTriggeredMe.GetInstanceID () != other.gameObject.GetInstanceID()) {
			HitManager.Instance.UpdateCoinText (pointsForCollider);
			gameObjectThatTriggeredMe = other.gameObject;
		
			switch(pointsForCollider){

			case 1:
				gameObjectThatTriggeredMe.transform.GetComponent<MeshRenderer> ().material.color = triggeredChangeColor1;
				break;

			case 2:
				gameObjectThatTriggeredMe.transform.GetComponent<MeshRenderer> ().material.color = triggeredChangeColor2;
				break;

			case 3:
				gameObjectThatTriggeredMe.transform.GetComponent<MeshRenderer> ().material.color = triggeredChangeColor3;
				break;



			}
		
		
		}

		//if (!isAllowPersist)
		//	other.gameObject.SetActive (false);
	
	}

	/*void OnTriggerExit(Collider other){

		if(isGround)
			HitManager.Instance.gONotInGround.Add (other.gameObject);

	}*/






	//void OnTriggerStay(Collider other){
		
//	if(gameObject.GetComponent <Collider>() == 
	/*	timer += Time.deltaTime;

		if (timer > timeForAirCoin) {
			UpdateCoinText ();
			timer = 0;
		}*/

	//}

	//void UpdateCoinText(){

	//	amountOfCoins += 1;

	//	coinText.text = ":" + amountOfCoins;
	
	//}
}
