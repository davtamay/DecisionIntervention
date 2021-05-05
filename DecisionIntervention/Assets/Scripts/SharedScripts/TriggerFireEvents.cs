using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerFireEvents : MonoBehaviour {

	[SerializeField]private UnityEvent onTriggerEnter;
	[SerializeField]private UnityEvent onTriggerExit;

    // [SerializeField] private bool isTriggerAfterSecond
    [Header("Wait For Initial Detection Settings")]
    [SerializeField] bool isWaitForDetection = false;
    [SerializeField] float waitTime;

    IEnumerator Start()
    {
        if (!isWaitForDetection)
            yield break;

        Debug.Log("THISISCOLLIDER");
        var thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;

        yield return new WaitForSecondsRealtime(waitTime);
        thisCollider.enabled = true;
        //isWaitForDetection = false;
        //onTriggerEnter.Invoke();
    }
    void OnTriggerEnter(Collider other){

        //if (isWaitForDetection)
        //    return;

        if (other.CompareTag("Player")){

			onTriggerEnter.Invoke();

		}
			
	}
	void OnTriggerExit(Collider other){

		if(other.CompareTag("Player")){

			onTriggerExit.Invoke();

		}
			
	}
}
