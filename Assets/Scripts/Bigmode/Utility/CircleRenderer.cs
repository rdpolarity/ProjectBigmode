using UnityEngine;

[ExecuteInEditMode]
public class CircleRenderer : MonoBehaviour
{
    public float Radius = 1f;
    [SerializeField] private LineRenderer lineRenderer;

    void DrawCircle() {
        int segments = 360;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        float x;
        float y;
        float z = 0f;
        float angle = 0f;
        for (int i = 0; i < segments + 1; i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * Radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * Radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, z));
            angle += (360f / segments);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawCircle();
    }
}
