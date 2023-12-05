using System.Collections;
using UnityEngine;

public class WiggleOnTouch : MonoBehaviour
{
    private bool isWobbling = false;
    public float wobbleAmount = 30f; // Maximum rotation angle for the wobble
    public float wobbleSpeed = 5f; // Speed of wobble
    public float wobbleDuration = 1f; // Duration of the wobble

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWobbling) return;
        // Prevent collision with self
        if (collision.gameObject.CompareTag("Grass")) return;
        StartCoroutine(WobbleEffect());
    }

    IEnumerator WobbleEffect()
    {
        isWobbling = true;
        float elapsedTime = 0f;
        float currentWobbleAmount = wobbleAmount;

        while (elapsedTime < wobbleDuration)
        {
            elapsedTime += Time.deltaTime;
            float angle = currentWobbleAmount * Mathf.Sin(wobbleSpeed * elapsedTime);
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            currentWobbleAmount = Mathf.Lerp(wobbleAmount, 0, elapsedTime / wobbleDuration);
            yield return null;
        }

        // Reset rotation to original
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        isWobbling = false;
    }
}
