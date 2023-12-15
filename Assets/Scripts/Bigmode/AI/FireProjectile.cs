using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FireProjectile : Action
{
    public GameObject projectilePrefab;
    public SharedGameObject target;

    public override TaskStatus OnUpdate()
    {
        if (projectilePrefab == null || projectilePrefab.GetComponent<Projectile>() == null)
            return TaskStatus.Failure;

        var projectile = Object.Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetTarget(target.Value.transform);

        return TaskStatus.Success;
    }
}
