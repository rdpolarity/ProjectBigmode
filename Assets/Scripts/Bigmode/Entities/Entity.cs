using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

namespace Bigmode
{
    public abstract class Entity : MonoBehaviour
    {

        public float MaxHealth { get; set; } = 10;

        private SpriteFlasher _damageFlasher;

        [SerializeField] private float health = 10;
        [SerializeField] private SoundEvent onDamageSound;
        [SerializeField] private SoundEvent onHealSound;

        virtual public float GetHealth()
        { return health; }

        virtual public void SetHealth(float value)
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
        virtual public void Damage(float amount)
        {
            SetHealth(GetHealth() - amount);
            _damageFlasher.Flash(Color.white);
            onDamageSound?.Play(transform);
        }

        [Button]
        virtual public void Heal(float amount)
        {
            SetHealth(GetHealth() + amount);
            _damageFlasher.Flash(Color.green);
            onHealSound?.Play(transform);
        }

        virtual public void Die()
        {
            Destroy(gameObject);
        }
    }
}