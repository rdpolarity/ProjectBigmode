using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class StartRuntimeNavmesh : MonoBehaviour
{
    public NavMeshSurface Surface2D;
    void Start()
    {
        Surface2D = GetComponent<NavMeshSurface>();
        Surface2D.BuildNavMeshAsync();
    }

    void Update() {
        // Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
