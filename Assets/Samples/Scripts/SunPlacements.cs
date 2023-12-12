using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

public class SunPlacements : MonoBehaviour
{
    [SerializeField] private GameObject[] sunPrefabs = null;
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
            
            var points = (Vector2Int[]) result["Points"];

            foreach (var point in points)
            {
                var scaledPoint = point;

                var randomPrefabIndex = Random.Range(0, sunPrefabs.Length);
                var prefab = sunPrefabs[randomPrefabIndex];
                var sun = Instantiate(prefab, transform, true);
                sun.transform.position = new Vector3(scaledPoint.x, scaledPoint.y, 0);
            }
        };
    }
}
