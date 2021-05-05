using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoboRyanTron.Unite2017.Events;
public class UIResiliencyManager : MonoBehaviour {


    [Header("References")]
	[SerializeField]private DataManager DATA_MANAGER;
    
    [SerializeField]
    private int currentResiliency;

    [Space][SerializeField]private Sprite highResiliencySprite, midResiliencySprite, lowResiliencySprite;


    private Text resiliencyPercentText;
    private Image resiliencyBar;

    private Text stressBar;
    private Image stressImage;
    

    void Awake(){


        if (resiliencyPercentText == null)
            resiliencyPercentText = GetComponentInChildren<Text>();

       
        if (resiliencyBar == null)
        {
            resiliencyBar = GetComponentInChildren<Image>();
            yBarSize = resiliencyBar.rectTransform.sizeDelta.y;
        }
        UpdateResiliencyUI();

        
    }
    public void OnEnable()
    {
        UpdateResiliencyUI();
    }


    public void UpdateResiliencyUI()
    {
        currentResiliency = DATA_MANAGER.playerData.playerResilienceHealth.resilienceHealth;
        

        if (currentResiliency >= 80)
        {
            resiliencyBar.color = Color.green;

        }
        else if (currentResiliency >= 40 && currentResiliency < 79)
        {
            resiliencyBar.color = Color.yellow;
        }
        else if (currentResiliency >= 0 && currentResiliency < 39)
        {
            resiliencyBar.color = Color.red;
          
        }

        resiliencyPercentText.text = currentResiliency + " " + "%";
        resiliencyBar.rectTransform.sizeDelta = new Vector2(currentResiliency, resiliencyBar.rectTransform.sizeDelta.y);

    }

    private float yBarSize;
   


    //void Update () {

    //   // UpdateStressText();

    //	if (isAddResiliency) {
    //        DATA_MANAGER.playerData.playerResilienceHealth.ApplyChange(5);
    //        isAddResiliency = false;
    //      //  UpdateStressText();
    //    }
    //	if (isReduceResiliency) {
        
    //        DATA_MANAGER.playerData.playerResilienceHealth.ApplyChange(-5);
    //        isReduceResiliency = false;
    //     //   UpdateStressText();
    //    }
      

    //}






}
