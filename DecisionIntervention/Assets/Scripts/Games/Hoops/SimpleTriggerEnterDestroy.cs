using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof (Collider))]
public class SimpleTriggerEnterDestroy : MonoBehaviour {


	void OnTriggerEnter(Collider other){

        //Destroy (other.transform.parent.parent.gameObject);
        if (other.CompareTag("Obstacle"))
            Destroy(other.gameObject);
      //  Destroy();

    }
}
