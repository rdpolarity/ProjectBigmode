using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Bigmode
{
    public abstract class Entity : MonoBehaviour
    {

        public float MaxHealth { get; set; } = 10;

        private SpriteFlasher _damageFlasher;

        [SerializeField] private float health = 10;

        public float GetHealth()
        { return health; }

        public void SetHealth(float value)
        {
            health = Math.Clamp(value, 0, MaxHealth);
            if (health <= 0) Die();
        }

        void Awake()
        {
            _damageFlasher = GetComponent<SpriteFlasher>();
            if (_damageFlasher == null)
                _damageFlasher = gameObject.AddComponent<SpriteFlasher>();
        }

        [Button]
        public void Damage(float amount)
        {
            SetHealth(GetHealth() - amount);
            _damageFlasher.Flash(Color.white);
        }

        [Button]
        public void Heal(float amount)
        {
            SetHealth(GetHealth() + amount);
            _damageFlasher.Flash(Color.green);
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}