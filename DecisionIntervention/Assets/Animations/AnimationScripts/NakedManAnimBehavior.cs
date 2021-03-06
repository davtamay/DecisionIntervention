using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NakedManAnimBehavior : StateMachineBehaviour {

	//  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsTag ("CloseShades")) {
			animator.transform.parent.GetChild (0).GetComponent<Animation> ().Play ();
			Destroy (animator.transform.gameObject, 4f);
		}

		if (stateInfo.IsTag ("GoDown"))
			animator.GetComponentInChildren<SkinnedMeshRenderer> ().SetBlendShapeWeight (3, 100);

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		//if (stateInfo.IsTag ("NakedWalk")) 


			
	//}


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		//if (stateInfo.IsTag ("NakedSurprise"))
			animator.SetTrigger ("GoDown");
		//if (animator.GetNextAnimatorStateInfo (0).IsTag ("CloseShades"))
		//if (stateInfo.IsTag ("CloseShades"))
		//	animator.transform.fi
		//	animator.transform.FindChild ("WindowShades").parent = animator.t;
		//	animator.transform.position += animator.transform.up * -0.8f * Time.deltaTime;

	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		if (stateInfo.IsTag ("NakedWalk")) {
			animator.transform.position += animator.transform.forward * 0.9f * Time.deltaTime;
			animator.transform.GetChild (0).localPosition = Vector3.zero;

		}
		if (stateInfo.IsTag ("GoDown")) 
			animator.transform.localPosition += animator.transform.up * -0.8f * Time.deltaTime;

		



	
	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
