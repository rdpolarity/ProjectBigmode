using UnityEngine;

namespace Bigmode
{
    public class Entity : MonoBehaviour
    {
        private Attributes attributes;
        void Start()
        {
            if (TryGetComponent<Attributes>(out attributes)) return;
            attributes = gameObject.AddComponent<Attributes>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}