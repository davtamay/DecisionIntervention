using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_UIManager : MonoBehaviour {

    private Image character_Image;
    private Text character_name;
    private bool isAvailable;
    [Header("Reference")]
   // public DataManager DATA_MANAGER;
    public Character_Profile character_Profile;
    public PlayerManager playerManager;
   // public Vector3Variable currentPlayerPosition;

    private void Start()
    {
        if (!character_Profile.isAvailable)
            gameObject.SetActive(false);

        character_Image = GetComponentInChildren<Image>();
        character_Image.sprite = character_Profile.characterImage;
        character_name = GetComponentInChildren<Text>();
        character_name.text = character_Profile.characterName;

    }

    public void SetLocation()
    {
       
        playerManager.ResetToCustomPosRotY(character_Profile.location);
      

    }
}
