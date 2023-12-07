using UnityEngine;

namespace Bigmode
{
    [ExecuteAlways]
    public class Billboard : MonoBehaviour
    {
        protected const float BillboardAngle = 0; // reset to 0 for now
        
        protected static Quaternion BillboardRotation => Quaternion.Euler(BillboardAngle, 0, 0);
        
        private void OnEnable()
        {
            transform.rotation = BillboardRotation;
        }
    }
}