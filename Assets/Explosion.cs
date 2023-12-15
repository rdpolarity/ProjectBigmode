using System.Collections;
using System.Collections.Generic;
using Bigmode;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private string damageTag = "Enemy";

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Start()
    {

        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        if (colliders.Length != 0)
        {
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<Entity>();
                if (entity != null && collider.CompareTag(damageTag))
                {
                    entity.Damage(damage);
                }
            }
        }

        var ps = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, ps.main.duration);
    }
}
