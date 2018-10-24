using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingScript : StateMachineBehaviour {
    private Vector3 RightSide = new Vector3(6.50f, -3.35f, 0f);
    private Vector3 leftSide = new Vector3(-6.4f, -3.35f, 0f);
    private Vector3 target;
    public float speed = 4f;
    int backToIdle = Animator.StringToHash("backToIdleTimer");
    private bool flipX;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        target = (target == RightSide) ? leftSide : RightSide;
        animator.gameObject.GetComponent<SpriteRenderer>().flipX = flipX;
        flipX = (flipX == false) ? true : false;


	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float step = speed * Time.deltaTime;
        // Move our position a step closer to the target.
        animator.gameObject.transform.position = Vector3.MoveTowards(animator.gameObject.transform.position, target, step);

        if (animator.gameObject.transform.position == target)
        {
            animator.SetBool(backToIdle, true);

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
