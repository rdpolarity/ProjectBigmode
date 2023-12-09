
using UnityEngine;

namespace Bigmode
{
    public class HealthSystem : MonoBehaviour
    {
        public bool Damage(float damage)
        {
            if (!TryGetComponent<Attributes>(out var attributes)) return false;
            var currentHealth = attributes.GetAttribute(Constants.Tags.Health);
            if (currentHealth.HasValue) attributes.SetAttribute(Constants.Tags.Health, currentHealth.Value - damage);
            else return false;

            return true;
        }

        public bool Heal(float heal)
        {
            if (!TryGetComponent<Attributes>(out var attributes)) return false;
            var currentHealth = attributes.GetAttribute(Constants.Tags.Health);
            if (currentHealth.HasValue) attributes.SetAttribute(Constants.Tags.Health, currentHealth.Value + heal);
            else return false;

            return true;
        }
    }
}