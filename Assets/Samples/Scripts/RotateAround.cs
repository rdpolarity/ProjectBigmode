using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Vector3 target = default;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Vector3 axis = Vector3.forward;
    
    private void Update()
    {
        transform.RotateAround(target, axis, Time.deltaTime * rotationSpeed);
    }
}
