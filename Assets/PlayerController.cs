using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator walkIdleAnimator;

    // Not pretty but using this to work out if we've moved as I'm not sure if we're gonna use rigid bodies?
    private float oldPositionX = -9999999; 
    private float oldPositionY = -9999999;

    public float MOVEMENT_ANIMATION_BUFFER; // if movement is less than this, will not draw walk animation

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if this isn't the first frame & we've moved a certain amount, set IsWalking for animation == true
        if (oldPositionX != -9999999 && 
            ( Math.Abs(oldPositionX - this.transform.position.x) + Math.Abs(oldPositionY - this.transform.position.y) >= MOVEMENT_ANIMATION_BUFFER ) )    
        {
            if (!walkIdleAnimator.GetBool("IsWalking"))
                walkIdleAnimator.SetBool("IsWalking", true);
        }
        else {
            if (walkIdleAnimator.GetBool("IsWalking"))
                walkIdleAnimator.SetBool("IsWalking", false);
        }

        oldPositionX = this.transform.position.x;
        oldPositionY = this.transform.position.y;
    }
}
