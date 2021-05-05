using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;


[CreateAssetMenu(fileName = "ResiliencyHealth", menuName = "CustomSO/PlayerData/ResiliencyHealth")]
public class ResiliencyData : ScriptableObject {

    [SerializeField]GameEvent onStressAdd;
    [SerializeField]GameEvent onStressReduce;

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public int resilienceHealth;

    public void SetValue(int value)
    {
        var isPositive = Mathf.Sign(value) == 1 ? true : false;

        if (value <= 0)
        {
         
            resilienceHealth = 0;
            value = 0;
            
        }
        if (value >= 100)
        {
         
            resilienceHealth = 100;
            value = 0;
            
        }

        //if (Mathf.Sign(value) == 1)
        //    onStressAdd.Raise();

        //else if (Mathf.Sign(value) == -1)
        //    onStressReduce.Raise();

       

        if (isPositive)
            onStressAdd.Raise();
        else
            onStressReduce.Raise();

        resilienceHealth = value;
    }

    //public void SetValue(IntVariable value)
    //{
    //    Value = value.Value;
    //}

    public void ApplyChange(int amount)
    {
        var isPositive = Mathf.Sign(amount) == 1? true : false;

        if (resilienceHealth + amount <= 0)
        {

            resilienceHealth = 0;
            amount = 0;
           
        }
        if (resilienceHealth + amount >= 100)
        {

            resilienceHealth = 100;
            amount = 0;
            
        }

        if (isPositive)
            onStressAdd.Raise();
        else
            onStressReduce.Raise();

        resilienceHealth += amount;
    }

    //public void ApplyChange(IntVariable amount)
    //{
    //    Value += amount.Value;
    //}

}
