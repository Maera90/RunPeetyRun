using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peety : MonoBehaviour
{
    Animator animator;
   
    public int JumpSpeed;
    public GameObject DustParticlesGO;
    public Vector2 DuckingCollider;
    public Vector2 DuckingColliderOffset;

    #region private
    private Animation anim;
    private Rigidbody2D rigidBody;
    private EnvironmentEngine environmentEngine;
    private ParticleSystem dustParticles;
    private BoxCollider2D boxCollider;
    private Vector2 OriginalBoxColliderSize;
    private Vector2 OriginalBoxColliderOffset;

    #endregion

    #region TempVariables
    private float lastDuckTime = 0;
    private float lastRushTime = 0;
    #endregion


    // Use this for initialization
    void Start ()
	{
	    animator = GetComponent<Animator>();
	    anim = GetComponent<Animation>();
	    rigidBody = GetComponent<Rigidbody2D>();
	    environmentEngine = GameObject.Find("EnvironmentEngine").GetComponent<EnvironmentEngine>();
	    dustParticles = DustParticlesGO.GetComponent<ParticleSystem>();
	    boxCollider = GetComponent<BoxCollider2D>();
	    OriginalBoxColliderSize = boxCollider.size;
	    OriginalBoxColliderOffset = boxCollider.offset;
	}
	
	// Update is called once per frame
    private float positionY = 0;
    
	void Update () {
     
     
	    //Manage Jumping animation
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


        //Set Running animation speed
        animator.speed = environmentEngine.speedMultiplicator * 1;
	    
        //Reset Duck Position
	    if (Time.time > (lastDuckTime+1) && animator.GetBool("IsDucking"))
	    {
	        animator.SetBool("IsDucking",false);
	        StopDucking();

	    }

        //Reset Rush Position
	    if (Time.time > (lastRushTime + 1) && animator.GetBool("IsRushing"))
	    {
            StopRushing();
	    }
	}

    public void Run()
    {
        animator.SetTrigger("IsRunning");
        animator.SetTrigger("GoesUp");
    }

    //Jump
    //Only available while not ducking or already in the air
    public void Jump()
    {
        if (animator.GetInteger("JumpState") == 0 && animator.GetBool("IsDucking") == false)
        {

            rigidBody.velocity = Vector2.up * JumpSpeed;
            
        }
       
    }

    //Ducking
    //Only available while not jumping
    public void Duck()
    {
        if (animator.GetInteger("JumpState") == 0)
        {
            animator.SetBool("IsDucking",true);
            lastDuckTime = Time.time;
            StartDucking();
        }
    }

    //Rushing
    //Only Available while not jumping or ducking
    public void Rush()
    {
        if (animator.GetInteger("JumpState") == 0 && animator.GetBool("IsDucking") == false)
        {
            animator.SetBool("IsRushing",true);
            lastRushTime = Time.time;
        }
    }

   

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject collisionGO = col.gameObject;
        if (collisionGO.CompareTag("Rock")||collisionGO.CompareTag("Meteor"))
        {
            environmentEngine.FinishGame();
            animator.SetBool("IsDead", true);

        }
    }

    void StartDucking()
    {     
        dustParticles.Play();
        Vector2 duckSize = new Vector2(OriginalBoxColliderSize.x, DuckingCollider.y);
        Vector2 duckOffset = new Vector2(OriginalBoxColliderOffset.x,DuckingColliderOffset.y);
        boxCollider.size = duckSize;
        boxCollider.offset = duckOffset;
    }

    void StopDucking()
    {
        dustParticles.Stop();
        boxCollider.size = OriginalBoxColliderSize;
        boxCollider.offset = OriginalBoxColliderOffset;
    }

    void StopRushing()
    {
        animator.SetBool("IsRushing",false);
    }
}
