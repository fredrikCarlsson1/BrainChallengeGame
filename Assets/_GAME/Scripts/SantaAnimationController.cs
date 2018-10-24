using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SantaAnimationController : MonoBehaviour {
    //ANIMATIONS
    Animator animator;
    int deadSantaHash = Animator.StringToHash("deadSanta");
    int walkingSantaHash = Animator.StringToHash("walkingSanta");
    int jumpingBackToIdle = Animator.StringToHash("jumpingBackToIdle");


    int slidingSanta = Animator.StringToHash("slidingSanta");

   
    private Vector3 RightSide = new Vector3(4.8f, -2f, 0f);
    private Vector3 leftSide = new Vector3(-5f, -2f, 0f);
    private Vector3 target;

    bool walkingToTheLeft = false;
    public bool isMoving = false;

    float speed = 3f;


    // Use this for initialization
    void Start()
    {
        
        animator = GetComponent<Animator>();
        target = RightSide;
	}


    public void SlideSanta() {
        //RUN TO ClOSEST TARGET
        isMoving = true;
        speed = 3f;
        //animator.SetTrigger(runningSanta);
        if (transform.position.x > 0) {
            
            target = RightSide;
            walkingToTheLeft = false;
        }
            
        else{
            target = leftSide;
            walkingToTheLeft = true;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = walkingToTheLeft;


    }

    public void RunningSanta() {
        speed = 3f;
    }

    public void WalkingSanta()
    {
        isMoving = true;
        speed = 1f;
    }   


    public void JumpingSanta() {
        isMoving = true;
        speed = 2f;
    }

    private float timer = 0;
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;


        if (isMoving) {
            timer = 0;
            transform.position = Vector3.MoveTowards(animator.gameObject.transform.position, target, step);
            if (transform.position == target) {
                target = (target == RightSide) ? leftSide : RightSide;
                walkingToTheLeft = (walkingToTheLeft == false) ? true : false;
                gameObject.GetComponent<SpriteRenderer>().flipX = walkingToTheLeft;
            }
        }
        else if (!isMoving) {
            timer += Time.deltaTime;
            if(timer>5){
                animator.SetTrigger(walkingSantaHash);
            }
                
        }




        //if (animator.gameObject.transform.position == target)
        //{
        //    animator.SetBool(backToIdle, true);

        //}

	}






}
