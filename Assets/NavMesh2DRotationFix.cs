using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh2DRotationFix : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.updateRotation = false;
		navMeshAgent.updateUpAxis = false;
        navMeshAgent.updatePosition = false;
    }

    private void Update()
    {
        var nextPosition = navMeshAgent.nextPosition;
        nextPosition.z = 0;
        transform.position = nextPosition;

        if (navMeshAgent.velocity != Vector3.zero)
        {
            // Apply the initial rotation
            transform.rotation = Quaternion.identity;
        }
    }
}
