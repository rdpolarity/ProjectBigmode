using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{
    [SerializeField, InfoBox("This will use the target from the parent VisionSystem component if it exists")]
    private bool useVisionSystem = false;
    [SerializeField, DisableIf("useVisionSystem")]
    private string targetTag = "Player";
    [SerializeField]
    private float speed = 2.5f;
    [SerializeField]

    private float stopFollowDistance = 2.5f;

    private Transform target;


    public void FindTarget(Animator animator)
    {
        if (useVisionSystem)
        {
            // get the parent
            var visionSystem = animator.GetComponent<VisionSystem>();
            target = visionSystem.target;
        }
        else
        {
            var objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);
            target = objectsWithTag.FirstOrDefault()?.transform;
        }

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindTarget(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindTarget(animator);

        if (target == null) return;

        var rigidbody2D = animator.GetComponent<Rigidbody2D>();
        if (Vector2.Distance(animator.transform.position, target.position) >= stopFollowDistance)
        {
            rigidbody2D.velocity = ((Vector2)target.position - rigidbody2D.position).normalized * speed;
        }
    }

}
