using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DragCamera : MonoBehaviour
{
    [SerializeField] private bool useBounds = true;
    [SerializeField] private RectInt bounds;

    private Camera cam;

    private Vector3 previousMouse;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousMouse = Input.mousePosition;
            return;
        }
        
        if (!Input.GetMouseButton(1)) return;

        var mouse = Input.mousePosition;
        
        var mouseWorld = cam.ScreenToWorldPoint(mouse);
        var prevWorld = cam.ScreenToWorldPoint(previousMouse);
        var mouseWorldDelta = mouseWorld - prevWorld;

        transform.Translate(-mouseWorldDelta);

        if (useBounds)
        {
            UpdateBounds();
        }
        
        previousMouse = mouse;
    }

    private void UpdateBounds()
    {
        var pos = transform.position;
        if (pos.x < bounds.xMin)
        {
            pos.x = bounds.xMin;
        }
        else if (pos.x > bounds.xMax)
        {
            pos.x = bounds.xMax;
        }
        
        if (pos.y < bounds.yMin)
        {
            pos.y = bounds.yMin;
        }
        else if (pos.y > bounds.yMax)
        {
            pos.y = bounds.yMax;
        }

        transform.position = pos;
    }

    public void SetBounds(int width, int height)
    {
        bounds.width = width;
        bounds.height = height;

        UpdateBounds();
    }
}
