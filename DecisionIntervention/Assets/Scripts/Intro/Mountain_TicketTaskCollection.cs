using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain_TicketTaskCollection : CollectTaskInteraction {

	[SerializeField] private string nameOfItemNeeded;
	[SerializeField] private Rigidbody gOToChangeIsKinematic;


	//public override void Start ()
	//{
	//	//PlayerPrefs.DeleteAll ();
	//	if (PlayerPrefs.GetInt (nameForPlayerPref) == 1) {
	//		gOToChangeIsKinematic.isKinematic = false;
	//		for (int i = 0; i < PlayerInventory.Instance.playerItemSlotGOList.Count; i++) {
			
	//			Debug.Log ("ITEMSLOTGOLIST" + i + PlayerInventory.Instance.playerItemSlotGOList [i]);
	//			if (string.Equals (PlayerInventory.Instance.playerItemSlotGOList [i].name, nameOfItemNeeded, System.StringComparison.CurrentCultureIgnoreCase))
	//				PlayerInventory.Instance.RemoveItemFromSlot (PlayerInventory.Instance.playerItemSlotGOList [i]);
	//		}
	//	}
	//}
	
	public override void OnTriggerEnter (Collider other)
	{
		//CheckForTaskCompletion ();
		return;
	}
	public override void OnTriggerExit (Collider other)
	{
		return;
	}
	public override void OnTriggerStay (Collider other)
	{
		return;
	}
	public override void CheckForTaskCompletion ()
	{
		//if (PlayerPrefs.GetInt (nameForPlayerPref) == 1)
		//	return;
		//for(int i = 0; i < PlayerInventory.Instance.playerItemSlotGOList.Count; i++){
	
		//	if (string.Equals (PlayerInventory.Instance.playerItemSlotGOList[i].name, nameOfItemNeeded, System.StringComparison.CurrentCultureIgnoreCase)) {

		//		gOToChangeIsKinematic.isKinematic = false;


		//		PlayerInventory.Instance.RemoveItemFromSlot (PlayerInventory.Instance.playerItemSlotGOList[i]);

		//		SaveTaskCompletion();
				
		//	}


		//}
	}

	
}
