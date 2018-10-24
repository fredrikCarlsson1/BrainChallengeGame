using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingToDeathScript : StateMachineBehaviour {
    private Vector3 target = new Vector3(-0.42f, -3f, 0f);
    public float speed = 2.5f;
    int deadSanta = Animator.StringToHash("deadSanta");
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float step = speed * Time.deltaTime;
        // Move our position a step closer to the target.
        animator.gameObject.transform.position = Vector3.MoveTowards(animator.gameObject.transform.position, target, step);

        if (animator.gameObject.transform.position == target)
        {
            animator.SetTrigger(deadSanta);

        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
