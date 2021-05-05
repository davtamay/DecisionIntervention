using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AreaUIManager : MonoBehaviour{

    [SerializeField] private Image childImage;
    [SerializeField] private Text childText;

    [Header("References")]
    [SerializeField]Area area_Profile;


    public void Start()
    {
        if (!area_Profile.isAvailable)
            gameObject.SetActive(false);

        childImage = transform.GetComponentInChildren<Image>();
        childText = transform.GetComponentInChildren<Text>();

        childImage.sprite = area_Profile.areaImage;
        childText.text = area_Profile.areaName;
        
    }

    //public void InitializeAreas(AreaList aL)
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        if (aL.Items[i].isAvailable)
    //        {
    //            childImage[i].sprite = aL.Items[i].areaImage;
    //            childText[i].text = aL.Items[i].areaName;
    //        }
    //        else
    //            childImage[i].gameObject.SetActive(false);

    //    }
    //}

}
