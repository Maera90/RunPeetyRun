using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peety : MonoBehaviour
{
    Animator animator;
    private Animation anim;
    private Rigidbody2D rigidBody;
    public int JumpSpeed;
    private EnvironmentEngine environmentEngine;

	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	    anim = GetComponent<Animation>();
	    rigidBody = GetComponent<Rigidbody2D>();
	    environmentEngine = GameObject.Find("EnvironmentEngine").GetComponent<EnvironmentEngine>();
	}
	
	// Update is called once per frame
    private float positionY = 0;
    
	void Update () {
     
     
	    
	    if(rigidBody.position.y < -2f)
	    {
	       
            
	        animator.SetInteger("JumpState",0);
	    }else if (rigidBody.position.y  > positionY )
	    {
            
	       animator.SetInteger("JumpState",1);
	    }
        else if (rigidBody.position.y < positionY)
	    {
	       
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
        if (animator.GetInteger("JumpState") == 0)
        {

            rigidBody.velocity = Vector2.up * JumpSpeed;
            
        }
       
    }

    void OnCollisionEnter2D(Collision2D col)
    {
       
    }

    void OnCollisionExit2D(Collision2D col)
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject collisionGO = col.gameObject;
        if (collisionGO.CompareTag("Rock"))
        {
            environmentEngine.gameFinished = true;
        }
    }
}
