using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Explode : Action
{
	// Gameobjects that have the explosion script
	public GameObject explosionPrefab;

	public override TaskStatus OnUpdate()
	{
		if (explosionPrefab == null || explosionPrefab.GetComponent<Explosion>() == null)
			return TaskStatus.Failure;

		var explosion = Object.Instantiate(explosionPrefab, transform.position, Quaternion.identity);

		return TaskStatus.Success;
	}
}