using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class VisionSystem : MonoBehaviour
{
    public List<Transform> targets = new();
    public Transform target;

    private Animator animator;
    private CircleCollider2D detectionRadiusCollider;

    [SerializeField]
    private List<string> targetTags = new() { "Player" };
    [SerializeField]
    private float detectionRadius = 5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        detectionRadiusCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionRadiusCollider.isTrigger = true;
        detectionRadiusCollider.radius = detectionRadius;
    }

    private void TargetsUpdated()
    {
        if (targets.IsNullOrEmpty())
        {
            animator.SetBool("isTargetInRange", false);
            target = null;
        }
        else
        {
            target = targets[0];
            animator.SetBool("isTargetInRange", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (targetTags.Contains(collider.gameObject.tag))
        {
            targets.Add(collider.gameObject.transform);
            TargetsUpdated();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (targetTags.Contains(collider.gameObject.tag))
        {
            targets.Remove(collider.gameObject.transform);
            TargetsUpdated();
        }
    }
}