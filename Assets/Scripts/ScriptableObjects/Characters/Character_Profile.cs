using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum Area_Names{City, Park, Mountain }
[CreateAssetMenu(fileName = "Character", menuName = "CustomSO/Character_Info")]
public class Character_Profile : ScriptableObject
{

    public bool isAvailable;
    public Vector3 location;
    public Sprite characterImage;
    public Area areaPresent;
    //public Area_Names areaPresent;
   // public string areaName;
    public string characterName;
}
