using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bigmode
{
    public class Biggie : Entity, IFriendly
    {
        [SerializeField]
        private int mass = 10;
        [SerializeField]
        private List<MinionType> minions = new();
        [SerializeField]
        private int selectMinion = 0;
        [SerializeField]
        private float sizePerMass = 0.1f;
        [SerializeField]
        private float healthPerMass = 1f;
        [SerializeField]
        private float spawnYDisplacement = 1f;

        void Start()
        {
            MassChanged();
        }

        void MassChanged()
        {
            MaxHealth = 1 + (mass * healthPerMass);
            var scale = 1 + (mass * sizePerMass);
            transform.localScale = Vector3.one * scale;
        }

        void IncreaseMass(int amount)
        {
            mass += amount;
            MassChanged();
        }

        void DecreaseMass(int amount)
        {
            mass -= amount;
            MassChanged();
        }


        /// <summary>
        /// Spends the specified amount of mass from the entity.
        /// </summary>
        /// <param name="amount">The amount of mass to spend.</param>
        /// <returns>True if the mass was successfully spent, false otherwise.</returns>
        bool SpendMass(int amount)
        {
            var finalMass = mass - amount;
            if (finalMass < 0) return false;

            DecreaseMass(amount);
            return true;
        }

        /// <summary>
        /// Selects a minion at the specified index.
        /// </summary>
        /// <param name="index">The index of the minion to select.</param>
        /// <returns>True if the minion was successfully selected, false otherwise.</returns>
        public bool SelectMinion(int index)
        {
            if (index < 0 || index >= minions.Count)
            {
                return false;
            }
            else
            {
                selectMinion = index;
                return true;
            }
        }

        public bool SpawnSelectedMinion()
        {
            var minionType = minions[selectMinion];
            if (SpendMass(minionType.cost))
            {
                // instantiate minion prefab at position
                var spawnPos = transform.position;
                spawnPos.y += spawnYDisplacement; // Displace spawn position below mouth
                Instantiate(minionType.prefab, transform.position, Quaternion.identity);
                return true;
            }
            else return false;
        }
    }
}