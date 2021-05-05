using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class PosAndRotEvent : UnityEvent<Vector3, Vector3> { }

public class SetPlayerPosAndRotToEvent : MonoBehaviour
{
    public PosAndRotEvent sendSourceRotAndPosEvent;

    [SerializeField] PlayerManager playerManager;

    [SerializeField] Vector3 sourcePos;
    [SerializeField] Vector3 sourceRot;

    //[SerializeField] Transform sourcePosAndRotObject;

    [SerializeField] Transform targetPosAndRotObject;

    //[SerializeField] bool isOnTarget;


   // [SerializeField] PlayerTransfomFunctions sourcePosRegisterForPosAndRot;

    public void Awake()
    {
        if (playerManager == null)
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        if (sendSourceRotAndPosEvent == null)
            sendSourceRotAndPosEvent = new PosAndRotEvent();
    }

    public void SetLocation()
    {
        
        Vector3 currentDirection = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        if (Mathf.Sign(currentDirection.y) == 0)
            playerManager.transform.eulerAngles += currentDirection;
        else
            playerManager.transform.eulerAngles -= currentDirection;

        Vector3 customRot = Vector3.zero;
        //if (!isOnTarget)
        //{
            //GRAB CURRENT TRANSFORM TO SAVE
            sourcePos = playerManager.transform.position;
            
            //playerManager.GetComponent<PlayerLookMove>().enabled = false;

            customRot = new Vector3(0, targetPosAndRotObject.eulerAngles.y, 0);

            playerManager.transform.eulerAngles += customRot;

            //SOURCEROT
            sourceRot = playerManager.transform.eulerAngles;


           playerManager.transform.position = targetPosAndRotObject.position;

            sendSourceRotAndPosEvent.Invoke(sourcePos, sourceRot);
           // sourcePosRegisterForPosAndRot.SetPosAndRotData(sourcePos, sourceRot);
               
           

           //  isOnTarget = true;
            
        }
        //else
        //{
           

        //   playerManager.GetComponent<PlayerLookMove>().enabled = true;

        //    customRot = new Vector3(0, sourceRot.y, 0);

        //  //TODO this doesnt go into last look  playerManager.transform.localEulerAngles += customRot;

        //    playerManager.transform.localPosition = sourcePos;

        //    isOnTarget = false;
        //}

    }
    //GVR EMULATOR RUNS ON UPDATE SO ANY MODIFICATIONS ON ROTTION HAS TO BE DONE AFTER IT?
    //IEnumerator LateStart()
    //{
    //    yield return new WaitForEndOfFrame();
    //    //   yield return new WaitForSecondsRealtime(0.2f);

    //    Vector3 currentDirection = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
     
    //    if (Mathf.Sign(currentDirection.y) == 0)
    //        playerManager.transform.eulerAngles += currentDirection;
    //    else
    //        playerManager.transform.eulerAngles -= currentDirection;

    //    Vector3 customRot = new Vector3(0,targetPosAndRotObject.eulerAngles.y,0);

    //    playerManager.transform.eulerAngles += customRot;
    
    //}
    

