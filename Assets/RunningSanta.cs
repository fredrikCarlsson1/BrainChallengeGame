using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSanta : StateMachineBehaviour {
    private Vector3 RightSide = new Vector3(4.8f, -2f, 0f);
    private Vector3 leftSide = new Vector3(-5f, -2f, 0f);
    int slidingSanta = Animator.StringToHash("slidingSanta");

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<SantaAnimationController>().SlideSanta();


	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.gameObject.transform.position == leftSide || animator.gameObject.transform.position == RightSide)
            animator.SetTrigger(slidingSanta);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
