using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "CustomSO/PlayerData/Item")]
public class Item : ScriptableObject {

	public string itemName;
	public Sprite itemSprite;
	public GameObject itemGO;
    public bool isPlayerCarrying;
    public bool isRemoveFromGame;
    public int gOID;

	void OnEnable(){
		//itemName = itemGO.name;
		gOID = itemGO.GetInstanceID ();


	}
}
