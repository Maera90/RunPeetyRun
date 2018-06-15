using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peety : MonoBehaviour
{
    Animator animator;
    private Animation anim;
    private Rigidbody2D rigidBody;
    public bool isJumping = false;

	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	    anim = GetComponent<Animation>();
	    rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
    private float positionY = 0;
    
	void Update () {
     
        Debug.Log(rigidBody.position.y);
	    
	    if(rigidBody.position.y < -2f)
	    {
	        Debug.Log("Running");
            isJumping = false;
	        animator.SetInteger("JumpState",0);
	    }else if (rigidBody.position.y  > positionY )
	    {
            Debug.Log("GoesUp");
	       animator.SetInteger("JumpState",1);
	    }
        else if (rigidBody.position.y < positionY)
	    {
	        Debug.Log("GoesDown");
	        animator.SetInteger("JumpState",2);
	    }
	    

        //velocityY = rigidBody.velocity.y;
        positionY = rigidBody.position.y;
    }

    public void Run()
    {
        animator.SetTrigger("IsRunning");
        animator.SetTrigger("GoesUp");
    }

    public void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            rigidBody.velocity = Vector2.up * 5;
            
        }
       
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Enter");
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("Exit");
    }
}
