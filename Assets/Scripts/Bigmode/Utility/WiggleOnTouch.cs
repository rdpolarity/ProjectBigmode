using UnityEngine;

namespace Bigmode
{
    public class WiggleOnTouch : Billboard
    {
        // Configuration
        [SerializeField, Tooltip("Maximum rotation angle for the wobble")] private float wobbleAmount = 30f;
        [SerializeField, Tooltip("Speed of wobble")] private float wobbleSpeed = 5f;
        [SerializeField, Tooltip("Duration of the wobble")] private float wobbleDuration = 1f;
        
        // Working vars
        private bool isWobbling;
        private float startedTime;
        private float currentWobble;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isWobbling) return;

            isWobbling = true;
            startedTime = Time.time;
            currentWobble = wobbleAmount;
        }
        
        private void Update()
        {
            if (!isWobbling)
            {
                transform.localRotation = BillboardRotation;
                return;
            }

            var elapsedTime = Time.time - startedTime;
            
            var angle = currentWobble * Mathf.Sin(wobbleSpeed * elapsedTime);
            
            transform.localRotation = Quaternion.Euler(BillboardAngle, 0, angle);
            currentWobble = Mathf.Lerp(wobbleAmount, 0, elapsedTime / wobbleDuration);

            if (elapsedTime > wobbleDuration)
                isWobbling = false;
        }
    }
}
