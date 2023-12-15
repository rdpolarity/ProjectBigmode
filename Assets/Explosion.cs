using System.Collections;
using System.Collections.Generic;
using Bigmode;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineImpulseDefinition;

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
        // Apply damage logic
        ApplyDamage();

        // Run impulse
        var impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource != null) impulseSource.GenerateImpulse();

        // Clean up
        var ps = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, ps.main.duration);
    }

    private void ApplyDamage()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            if (entity != null && collider.CompareTag(damageTag))
            {
                entity.Damage(damage);
            }
        }
    }
}
