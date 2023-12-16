using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DespawnEnemy : Action
{
	public override TaskStatus OnUpdate()
	{
		SpawnManager.Instance.DespawnEntity(gameObject);
		return TaskStatus.Success;
	}
}