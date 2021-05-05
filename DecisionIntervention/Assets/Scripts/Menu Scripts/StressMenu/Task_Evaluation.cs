using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Task_Evaluation : MonoBehaviour {

    
    [SerializeField] UnityEvent onIdentified;
    [SerializeField] UnityEvent onCompleted;

    [Space]
    
    [Header("Task_Evaluate_Option = Item_Carry")]
    [SerializeField] bool evaluateItemCarry = true;
    [SerializeField] Transform parentOfItemsForCompletion;

    [Space]

    [SerializeField] bool isGiveItemOnCompletion = false;
    [SerializeField] Item itemToGiveOnCompletion;


    [Space]
    
    [Header("References")]
    [SerializeField] DataManager DATA_MANAGER;
    [SerializeField] Task task;

    public void Start()
    {
        EvaluateTaskStatus();
    }

    public void EvaluateTaskStatus()
    {

        switch (task.taskStatus)
        {
            case Task_Status.IDENTIFIED:
                onIdentified.Invoke();

                break;
            case Task_Status.COMPLETED:
                onCompleted.Invoke();
                break;

        }
    }
    public void EvaluatePlayerItemCarry()
    {

        int itemsCollected = 0;
        int cCount = parentOfItemsForCompletion.childCount;

        Item[] itemList = new Item[cCount];

        foreach (var item in DATA_MANAGER.playerData.masterInventoryList.Items)
        {
            

            for (int i = 0; i < cCount; i++)
            {

                if (string.Equals(item.itemName, parentOfItemsForCompletion.GetChild(i).gameObject.name, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    ++itemsCollected;
                    itemList[i] = item;
                    //PlayerInventory.Instance.RemoveItemFromSlot(item);
                }

            }
        }

        if (cCount == itemsCollected)
        {
            for (int t = 0; t < itemsCollected; t++)
            {
                itemList[t].isPlayerCarrying = false;
                itemList[t].isRemoveFromGame = true;
                PlayerInventory.Instance.RemoveItemFromSlot(itemList[t]);
            }
            if (isGiveItemOnCompletion)
                PlayerInventory.Instance.AddItemToSlot(itemToGiveOnCompletion);
           
            
            SaveTaskCompletion();
        }
    }

    public void SaveTaskIdentified()
    {
        if(task.taskStatus == Task_Status.NOT_IDENTIFIED)
      //  if (PlayerPrefs.HasKey(task.playerPrefs_SaveName) == false)
        {
            task.taskStatus = Task_Status.IDENTIFIED;
            // PlayerPrefs.SetInt(task.playerPrefs_SaveName, 0);
            //  PlayerPrefs.Save();
            onIdentified.Invoke();

            QuestAssess.Instance.OnUpdate();


        }
        //
    }

    public bool IsTaskIdentified()
    {
        if (task.taskStatus == Task_Status.IDENTIFIED)
            return true;

        return false;

        //if (PlayerPrefs.HasKey(task.playerPrefs_SaveName) == true)
        //    return true;

        //return false;

    }

    public void SaveTaskCompletion()
    {
        if (task.taskStatus == Task_Status.IDENTIFIED)
        {
            task.taskStatus = Task_Status.COMPLETED;
            //PlayerPrefs.SetInt(task.playerPrefs_SaveName, 1);
            //PlayerPrefs.Save();
            onCompleted.Invoke();
            QuestAssess.Instance.OnUpdate();
        }


    }
    public bool IsTaskCompleted()
    {
        if (task.taskStatus == Task_Status.COMPLETED)
            return true;

        return false;

    }
    public Task_Status GetCurrentTaskEvaluationState()
    {

        return task.taskStatus;
    }
}
