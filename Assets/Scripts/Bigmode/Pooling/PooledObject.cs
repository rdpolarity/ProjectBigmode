using UnityEngine;

namespace Bigmode
{
    public class PooledObject: MonoBehaviour
    {
        [HideInInspector] public GameObject prefab;
        [HideInInspector] public Vector3Int cell;

        private void OnDisable()
        {
            Pool.Return(this);
        }
    }
}