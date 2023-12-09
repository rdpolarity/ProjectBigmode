// using System.Collections;
using UnityEngine;

public class FollowController : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, 0.1f);
    }
}
