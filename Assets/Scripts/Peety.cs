﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peety : MonoBehaviour
{
    Animator animator;
    private Animation anim;
    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	    anim = GetComponent<Animation>();
	    rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
    private float velocityY = 0;
	void update () {
     
        Debug.Log(velocityY*Time.deltaTime);
	    if (rigidBody.velocity.y  > velocityY)
	    {

	        if (!animator.GetBool("GoesUp"))
	        {
	            animator.SetBool("GoesUp",true);
	        }
	        if (animator.GetBool("GoesDown"))
	        {
	            animator.SetBool("GoesDown",false);
	        }
	       
            

	    }else if (rigidBody.velocity.y < velocityY)
	    {
	        if (animator.GetBool("GoesUp"))
	        {
	            animator.SetBool("GoesUp",false);
	        }
	        if (!animator.GetBool("GoesDown"))
	        {
	            animator.SetBool("GoesDown",true);
	        }
	    }
	    else
	    {
	        if (animator.GetBool("GoesUp"))
	        {
	            animator.SetBool("GoesUp",false);
	        }
	        if (animator.GetBool("GoesDown"))
	        {
	            animator.SetBool("GoesDown",false);
	        }
	    }

	    velocityY = rigidBody.velocity.y;
        


	}

    public void Run()
    {
        animator.SetTrigger("IsRunning");
        animator.SetTrigger("GoesUp");
    }

    public void Jump()
    {
        rigidBody.velocity = Vector2.up * 5;
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
