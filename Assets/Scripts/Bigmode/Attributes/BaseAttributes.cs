using System;
using UnityEngine;

namespace Bigmode
{
    public class BaseAttributes : Attributes
    {

        [SerializeField]
        private float baseHealth = 100f;
        [SerializeField]
        private float baseMaxHealth = 100f;
        [SerializeField]
        private float baseSpeed = 5f;
        [SerializeField]
        private float baseDamage = 10f;

        void Start()
        {
            SetAttribute(Constants.Tags.Health, baseHealth);
            SetAttribute(Constants.Tags.MaxHealth, baseMaxHealth);
            SetAttribute(Constants.Tags.Speed, baseSpeed);
            SetAttribute(Constants.Tags.Damage, baseDamage);
        }

        public override float PreAttributeChange(string name, float value)
        {
            switch (name)
            {
                // Clamp health to max health
                case Constants.Tags.Health:
                    var currentMaxHealth = GetAttribute(Constants.Tags.MaxHealth);
                    if (currentMaxHealth != null) value = Mathf.Clamp(value, 0, (float)currentMaxHealth);
                    break;
            }
            return value;
        }
    }
}