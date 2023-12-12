using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

[ExecuteInEditMode]
public class StarMapConnections : MonoBehaviour
{
    [SerializeField] private Material lineMaterial = null;
    [SerializeField] private ScriptGraphRunner graphRunner = null;

    private void Start()
    {
        graphRunner.OnProcessed += result =>
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                if (child == transform) continue;
                
                DestroyImmediate(child.gameObject);
            }
            
            var connectedPointPairs = (Pair<Vector2Int>[]) result["Connected Points"];

            foreach (var pair in connectedPointPairs)
            {
                var lineObject = new GameObject("Connection Line");
                var line = lineObject.AddComponent<LineRenderer>();

                var first = pair.First;
                var second = pair.Second;

                line.sortingOrder = 1;
                line.positionCount = 2;
                line.SetPosition(0, new Vector3(first.x, first.y, 0));
                line.SetPosition(1, new Vector3(second.x, second.y, 0));
                line.startWidth = .2f;
                line.endWidth = .2f;
                line.useWorldSpace = false;
                line.material = lineMaterial;
                
                lineObject.transform.SetParent(transform);
            }
        };
    }
}
