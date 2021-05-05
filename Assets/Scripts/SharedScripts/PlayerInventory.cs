using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	private Dictionary<GameObject,SpriteRenderer> totalGOToSpriteInSceneDict;
	private Dictionary<int, SpriteRenderer> playerIntToSpriteUISlotsDict;

	public Dictionary<string, GameObject> StringToGODict;

	//public List<GameObject> playerItemSlotGOList;

	[SerializeField]private GameObject UIItemSlotsParent;
	public List<SpriteRenderer> slotSpots;

	private int curSlot;

	private string curSceneName;

	private GameObject itemContainer;

	//[SerializeField] private InventoryList masterPlayerInventoryList;
	[SerializeField] private DataManager DATA_MANAGER;

	//public Dictionary<string,GameObject> allItemsInScene;

	private static PlayerInventory instance;
	public static PlayerInventory Instance {
		get{ return instance; }
	}

	public void Awake(){
	

		if (instance != null) {
			Debug.LogError ("There are two instances off PlayerInventory");
			return;
		} else {
			instance = this;
		}

		if(SceneController.Instance != null)
			curSceneName = SceneController.Instance.GetCurrentSceneName ();
	
	}


	void Start()
	{

		foreach(Transform s in UIItemSlotsParent.transform)
			slotSpots.Add (s.GetComponent<SpriteRenderer> ());

        foreach (var item in DATA_MANAGER.playerData.masterInventoryList.Items)
        {
            
            if (!item.isPlayerCarrying || item.isRemoveFromGame)
                continue;

            foreach (var sSR in slotSpots)
                if (sSR.sprite == null)
                {
                    sSR.sprite = item.itemSprite;
  //FIXME how do I keep references to objects to deactivate them on scene initiation                  item.itemGO.SetActive(false); 
               //     allItemsInScene[item.itemName].SetActive(false);
                    //	item.itemGO.SetActive (false);
                    break;
                }
        }
				
			
		

       


//
//		//FIXME Games may use Item game tag besided the intro, so check if there are no interuptions/bugs 5/13/17
//		//		if(SceneController != null)
//		if(curSceneName == "Intro")
//		if (GameObject.FindWithTag ("Item") != null) {
//
//			allItemGOInScene = GameObject.FindGameObjectsWithTag ("Item");
//
//			totalGOToSpriteInSceneDict = new Dictionary<GameObject,SpriteRenderer> ();
//			playerIntToSpriteUISlotsDict = new Dictionary<int,SpriteRenderer> ();
//			playerItemSlotGOList = new List<GameObject> ();
//			StringToGODict = new Dictionary<string, GameObject>();
//
//			foreach(GameObject GO in allItemGOInScene){
//
//				SpriteRenderer curSprRend = GO.GetComponentInChildren<SpriteRenderer> (true);//GO.transform.GetChild (0).transform.GetChild (0).GetComponentInChildren<SpriteRenderer> ();
//				//Debug.Log(curSprRend.sprite.name);
//
//				totalGOToSpriteInSceneDict.Add (GO, curSprRend);
//
//				StringToGODict[GO.name] =  GO;
//
//				Debug.Log ("GOInEnvironment: " + GO.name);
//			}
//
//
//			int itemCount = 0;
//
//
//			foreach (Transform curTran in UISlots.transform) {
//
//				playerIntToSpriteUISlotsDict.Add(itemCount, curTran.GetComponentInChildren<SpriteRenderer>(true));
//
//				itemCount++;
//
//				//Debug.Log ("SLOTS:" + curTran.name);
//				curTran.gameObject.SetActive (false);
//
//			}
//
//			//Needs to initiate before using liststringnames
//			int loadItemListCount = DataManager.Instance.LoadItemList ().Count;
//
//			foreach (GameObject gO in DataManager.Instance.LoadItemList ())
//				Debug.Log("LOADED LIST + : " + gO.name);
//
//
//			if (loadItemListCount != 0) {
//				playerItemSlotGOList.Clear ();
//				for (int i = 0; i < loadItemListCount ; i++) {
//
//					if (!playerItemSlotGOList.Contains (StringToGODict[DataManager.Instance.slotListItemNames[i]])) {
//
//						playerItemSlotGOList.Add (StringToGODict[DataManager.Instance.slotListItemNames[i]]);
//						AddItemToSlot (StringToGODict[DataManager.Instance.slotListItemNames[i]]);
//
//
//						StringToGODict[DataManager.Instance.slotListItemNames[i]].SetActive (false);
//
//
//					}
//
//				}
//			}
//
//		}


	}
    
	public void AddItemToSlot(Item itemToAdd)
	{

		foreach(var sSR in slotSpots)
		{

			if(sSR.sprite == null)
			{

				foreach(var item in DATA_MANAGER.playerData.masterInventoryList.Items)
				{
                    
                   if(string.Equals(item.itemName, itemToAdd.itemName, System.StringComparison.CurrentCultureIgnoreCase)) { 
					
                        itemToAdd.isPlayerCarrying = true;
                        sSR.sprite = item.itemSprite;
                        itemToAdd.itemGO.SetActive(false);
                       
                        return;
                    }
					

				}

			}

		}

        
	}

    

	public void RemoveItemFromSlot(Item itemToRemove)
	{
		foreach (var sSR in slotSpots) 
		{

			if (sSR.sprite != null) 
			{
				
				foreach (var item in DATA_MANAGER.playerData.masterInventoryList.Items) 
				{


                    if (string.Equals(item.itemName, itemToRemove.itemName, System.StringComparison.CurrentCultureIgnoreCase))
					{
                      

                        sSR.sprite = null;
                       // sSR.gameObject.SetActive(false);
                       

                     
					}

				}
			}
		}
//		GameObject GO = StringToGODict [gO.name];
//
//
	}

	//public void OnApplicationQuit(){

	//	if(curSceneName.Contains("Intro"))
	//		DATA_MANAGER.SaveItemList (playerItemSlotGOList);
	
	//}


}
