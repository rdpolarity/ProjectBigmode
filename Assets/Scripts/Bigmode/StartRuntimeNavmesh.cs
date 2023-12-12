using System.Collections;
using NavMeshPlus.Components;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

// This script will update the navmesh at runtime
public class StartRuntimeNavmesh : MonoBehaviour
{
    public NavMeshSurface Surface2D;
    public Transform Player;
    public bool UseUpdateRadius = false;
    private Vector2 lastPosition = Vector2.zero;
    public float UpdateRadius = 5f;
    private bool isBuildingNavMesh = false;
    public float UpdateCooldown = 2f;
    private float lastUpdateTime = 0f;

    void Start()
    {
        Surface2D = GetComponent<NavMeshSurface>();
        Surface2D.BuildNavMeshAsync();
    }

    public void RequestNavMeshUpdate() {
        UpdateNavMesh();
    }

    void Update()
    {
        if (!UseUpdateRadius) return;
        // // If the player has moved more than the update radius, update the navmesh
        if (Vector2.Distance(Player.position, lastPosition) > UpdateRadius && !isBuildingNavMesh && Time.time > lastUpdateTime + UpdateCooldown)
        {
            StartCoroutine(UpdateNavMesh());
            lastPosition = Player.position;
            lastUpdateTime = Time.time;
        }
    }

    IEnumerator UpdateNavMesh()
    {
        isBuildingNavMesh = true;
        yield return Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        isBuildingNavMesh = false;
    }
}
