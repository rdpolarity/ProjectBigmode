using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

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
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private LineRenderer tongueRenderer;
        [SerializeField]
        private float maxTongueLength = 2f;

#nullable enable
        private Mass? grabbedMass = null;

        void Start()
        {
            MassChanged();
            SelectMinion(selectMinion);
            // create tongue rendere
            if (tongueRenderer == null) tongueRenderer = gameObject.AddComponent<LineRenderer>();
            tongueRenderer.startColor = Color.red;
            tongueRenderer.endColor = Color.red;
            // set width 0.1
            tongueRenderer.startWidth = 0.1f;
            tongueRenderer.endWidth = 0.05f;
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
                spriteRenderer.sprite = minions[selectMinion].sprite;
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
                Instantiate(minionType.prefab, transform.position, transform.rotation);

                return true;
            }
            else return false;
        }

        public void TongueGrab()
        {

            // find objects in physics 2D overlap circle
            var colliders = Physics2D.OverlapCircleAll(transform.position, maxTongueLength);

            var masses = new List<Mass>();
            // filter out everything but Minions and Fly tags
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Mass>(out var mass))
                {
                    masses.Add(mass);
                }
            }

            if (masses.Count == 0) return;

            // sort by distance
            masses.Sort((a, b) =>
            {
                var aDist = Vector3.Distance(a.transform.position, transform.position);
                var bDist = Vector3.Distance(b.transform.position, transform.position);
                return aDist.CompareTo(bDist);
            });

            grabbedMass = masses[0];
        }

        public void TongueRelease()
        {
            if (grabbedMass == null) return;

            IncreaseMass(grabbedMass.mass);
            Destroy(grabbedMass.gameObject);

            grabbedMass = null;
        }

        void Update()
        {
            if (grabbedMass != null)
            {
                tongueRenderer.enabled = true;
                var start = tongueRenderer.transform.position;
                var end = grabbedMass.transform.position;
                tongueRenderer.SetPositions(new Vector3[] { start, end });

                // Calculate the distance between the player and the minion
                float distance = Vector3.Distance(start, end);

                // If the minion is further away than the max tongue length, pull it closer
                if (distance > maxTongueLength)
                {
                    // Move the minion towards the player
                    grabbedMass.transform.position = Vector3.MoveTowards(end, start, distance - maxTongueLength);
                }
            }
            else
            {
                tongueRenderer.enabled = false;
            }
        }
    }
}