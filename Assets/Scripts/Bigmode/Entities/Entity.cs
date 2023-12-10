using System;
using UnityEngine;

namespace Bigmode
{
    public abstract class Entity : MonoBehaviour
    {

        public float Health
        {
            get { return Health; }
            set { Health = Math.Clamp(value, 0, MaxHealth); }
        }

        public float MaxHealth { get; set; }

        void Damage(float amount)
        {
            Health -= amount;
        }

        void Heal(float amount)
        {
            Health += amount;
        }
    }
}