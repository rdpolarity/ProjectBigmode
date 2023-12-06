using UnityEngine;

namespace Bigmode
{
    [ExecuteAlways]
    public class Billboard : MonoBehaviour
    {
        protected const float BillboardAngle = -15f;
        
        protected static Quaternion BillboardRotation => Quaternion.Euler(BillboardAngle, 0, 0);
        
        private void OnEnable()
        {
            transform.rotation = BillboardRotation;
        }
    }
}