using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;


public class PlayerTransfomFunctions : MonoBehaviour
{
   
    

    public Vector3 currentSavedPosition;
    public Vector3 currentSavedRotation;

    [Header("References")]
    [SerializeField] private PlayerManager playerManager;

    public void Awake()
    {
       
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

    }

    public void SetPosAndRotData(Vector3 pos, Vector3 rot)
    {

        currentSavedPosition = pos;
        currentSavedRotation = rot;

    }
   

    public void Invoke_Transform()
    {
          playerManager.transform.position = currentSavedPosition;



        Vector3 currentDirection = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        if (Mathf.Sign(currentDirection.y) == 0)
            playerManager.transform.eulerAngles += currentDirection;
        else
            playerManager.transform.eulerAngles -= currentDirection;


        Vector3 customRot = Vector3.zero;
        customRot = new Vector3(0, currentSavedRotation.y, 0);

        playerManager.transform.eulerAngles += Mathf.Sign(currentDirection.y) == 0 ? customRot : -customRot;



       // playerManager.transform.eulerAngles = currentSavedRotation;
    }
}
