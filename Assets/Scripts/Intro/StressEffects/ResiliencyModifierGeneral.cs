using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResiliencyModifierGeneral : MonoBehaviour {

    [SerializeField] private bool isOffFromStart = false;

	[SerializeField]private float stressEffect;
	[SerializeField]private float timeUntilStressEffect;

    [SerializeField] private bool isAffectDuringUnscaledTime =false;

    [SerializeField] private bool isTriggerAffect = false;
    [Header("References")]
    [SerializeField] private DataManager DATA_MANAGER;

    float timer = 0;

    public void Awake()
    {
        if (isOffFromStart)
            enabled = false;
      
        //isOffFromStart = true;
    }

    private Coroutine EffectCoroutine;
    public bool isStressCoroutineRunning = false;

    public void TurnOnStressEffect()
    {
        // EffectCoroutine = 
        isStressCoroutineRunning = true;
        StartCoroutine(EffectUpdate());
        
    
    }
   
    public void TurnOffStressEffect()
    {
    //   StopCoroutine(EffectCoroutine);
       isStressCoroutineRunning = false;

    }
    IEnumerator EffectUpdate()
    {
        while (isStressCoroutineRunning)
        {
            if (isTriggerAffect)
                yield break;

            if (isAffectDuringUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                timer += Time.unscaledDeltaTime;//deltaTime;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                timer += Time.deltaTime;//
            }


            if (timeUntilStressEffect < timer)
            {
                Debug.Log(this.name + " - stress Is RUNNING");
                DATA_MANAGER.playerData.playerResilienceHealth.ApplyChange((int)stressEffect);
                timer = 0;
            }
        }
    }
    void Update () {

        if (isTriggerAffect)
            return;

        if(isAffectDuringUnscaledTime)
            timer += Time.unscaledDeltaTime;//deltaTime;
        else
            timer += Time.deltaTime;//


        if (timeUntilStressEffect < timer) {
			Debug.Log (this.name + " - stress Is RUNNING");
            DATA_MANAGER.playerData.playerResilienceHealth.ApplyChange((int)stressEffect);
            //IStressGage.Instance.stress = stressEffect;
			timer = 0;
		}


	}

    public void OnTriggerEnter(Collider other)
    {
        if(isTriggerAffect)
        {

            DATA_MANAGER.playerData.playerResilienceHealth.ApplyChange((int)stressEffect);
        }
    }


}
