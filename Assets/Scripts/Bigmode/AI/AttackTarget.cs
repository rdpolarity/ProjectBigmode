using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Bigmode;

public class AttackTarget : Action
{
    public float damage = 1;
    public SharedGameObject target;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
            return TaskStatus.Failure;

        var entity = target.Value.GetComponent<Entity>();
        entity.Damage(damage);

        return TaskStatus.Success;
    }
}
