using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator walkIdleAnimator;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb) return;

        bool isMoving = rb.velocity.magnitude > 0f;
        bool isWalkingAnimationVar = walkIdleAnimator.GetBool("IsWalking");
        if (isMoving) walkIdleAnimator.SetBool("IsWalking", true);
        else walkIdleAnimator.SetBool("IsWalking", false);
    }
}
