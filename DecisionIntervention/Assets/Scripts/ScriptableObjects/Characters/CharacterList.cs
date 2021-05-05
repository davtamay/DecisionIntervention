using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_ProfileManager : MonoBehaviour {

    private Image character_Image;
    private Text character_name;
    private bool isAvailable;
    [Header("Reference")]
    public List<Character_Profile> character_ProfileList;
    public PlayerManager playerManager;
    public Vector3Variable currentPlayerPosition;

    private void Start()
    {
        //character_Image = GetComponentInChildren<Image>();
        //character_Image.sprite = character_info.characterImage;

        //character_name = GetComponentInChildren<Text>();
        //character_name.text = character_info.characterName;

    }

    public void SetLocation()
    {
        // playerManager.ResetPositionToHome();
        // currentPlayerPosition.Value = character_info.location;

        //playerManager.ResetToCustomPosition(character_ProfileList.location);


    }

}
