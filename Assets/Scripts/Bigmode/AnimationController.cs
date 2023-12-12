using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float walkingThreshold = 0.2f;
    [SerializeField] private float speedAnimationMultiplier = 0.8f;

    private new Rigidbody2D rigidbody;
    private Animator walkIdleAnimator;
    private NavMeshAgent navMeshAgent;

    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null) rigidbody = GetComponentInParent<Rigidbody2D>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) navMeshAgent = GetComponentInParent<NavMeshAgent>();

        walkIdleAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isMoving = false;
        float speed = 0;
        
        if (navMeshAgent != null)
        {
            isMoving = navMeshAgent.velocity.magnitude > walkingThreshold;
            walkIdleAnimator.SetBool(IsWalking, isMoving);

            speed = navMeshAgent.velocity.magnitude;
        }
        else if (rigidbody != null)
        {
            isMoving = rigidbody.velocity.magnitude > walkingThreshold;
            speed = rigidbody.velocity.magnitude;
        };

        walkIdleAnimator.SetBool(IsWalking, isMoving);

        if (isMoving) walkIdleAnimator.speed = speed * speedAnimationMultiplier;
        else walkIdleAnimator.speed = 1;
    }
}
