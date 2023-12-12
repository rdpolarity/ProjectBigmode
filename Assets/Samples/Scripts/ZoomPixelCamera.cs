using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PixelPerfectCamera))]
public class ZoomPixelCamera : MonoBehaviour
{
    [SerializeField] private int minPpu = 16;
    [SerializeField] private int maxPpu = 32;
        
    private PixelPerfectCamera cam;
    
    private void Start()
    {

        cam = GetComponent<PixelPerfectCamera>();
    }

    private void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) < .0001) return;

        if (Input.mouseScrollDelta.y > 0f)
        {
            cam.assetsPPU *= 2;
        }
        else
        {
            cam.assetsPPU /= 2;
        }

        if (cam.assetsPPU > maxPpu)
        {
            cam.assetsPPU = maxPpu;
        }
        else if (cam.assetsPPU < minPpu)
        {
            cam.assetsPPU = minPpu;
        }
    }
}
