using UnityEngine;

public class DebugWalk : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the character

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from keyboard or controller
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Apply the movement to the Rigidbody2D
        if (moveX == 0 && moveY == 0)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            Vector2 move = new Vector2(moveX, moveY).normalized * moveSpeed;
            rb.velocity = move;
        }
    }
}

