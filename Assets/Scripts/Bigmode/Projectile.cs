using Bigmode;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private bool isHoming = false;
    [SerializeField] private Transform target;
    [SerializeField] private string targetTag = "Enemy";

    private float lifeTimer;
    private Rigidbody2D rb;

    void Start()
    {
        lifeTimer = lifeTime;
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (!isHoming || target == null)
        {
            rb.velocity = transform.right * speed;
        }
    }
    void FixedUpdate()
    {
        if (isHoming && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    void Update()
    {
        // Update the life timer and destroy the projectile if time runs out
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("Projectile collided with " + other.name);
        if (!other.CompareTag(targetTag)) return;
        other.GetComponent<Entity>()?.Damage(damage);
        Destroy(gameObject); // Destroy the projectile upon collision
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

}
