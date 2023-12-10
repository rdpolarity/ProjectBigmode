using System.Collections;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{

    [SerializeField]
    private float patrolRadius = 5f;
    [SerializeField]
    private float minWaitTime = 1f;
    [SerializeField]
    private float maxWaitTime = 5f;
    private Transform transform;

    private Vector2? patrolPoint;
    private bool isMovingToPatrolPoint = false;
    private Rigidbody2D rb;
    private float waitTime;
    private float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
        rb = animator.GetComponent<Rigidbody2D>();
        SetNewPatrolPoint();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isMovingToPatrolPoint)
        {
            if (patrolPoint == null)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    SetNewPatrolPoint();
                }
            }
        }

        if (patrolPoint.HasValue)
        {
            MoveTowardsPatrolPoint();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        patrolPoint = null;
    }

    private void SetNewPatrolPoint()
    {
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        patrolPoint = (Vector2)transform.position + new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * patrolRadius;
        isMovingToPatrolPoint = true;
    }

    private void MoveTowardsPatrolPoint()
    {
        if (patrolPoint.HasValue)
        {
            Vector2 direction = (patrolPoint.Value - (Vector2)transform.position).normalized;
            float speed = 2f; // Set your desired speed

            rb.velocity = direction * speed;

            if (Vector2.Distance(transform.position, patrolPoint.Value) < 0.1f)
            {
                isMovingToPatrolPoint = false;
                rb.velocity = Vector2.zero; // Stop the movement
                patrolPoint = null; // Reset patrol point
                waitTime = Random.Range(minWaitTime, maxWaitTime);
                timer = waitTime;
            }
        }
    }
}
