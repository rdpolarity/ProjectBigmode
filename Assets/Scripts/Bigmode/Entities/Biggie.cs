using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sonity;
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
        private float cameraZoomOutPerMass = 0.1f;
        [SerializeField]
        private float spawnYDisplacement = 1f;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private LineRenderer tongueRenderer;
        [SerializeField]
        private GameObject dogePrefab;

        public List<IMassChangeListener> massChangeListeners = new();

        public List<IMinionCountChangeListener> minionCountChangeListeners = new();

        public int AdditionalMassPerFly = 0;

        [SerializeField]
        private float baseMaxTongueLength = 2f;
        [SerializeField]
        private GameObject mouthSprite;
        [SerializeField]
        private int maxSize = 200;
        [SerializeField]
        private float rbMassPerFly = 0.01f;

        public delegate void OnSelectedMinionChanged(MinionType minion);
        public static event OnSelectedMinionChanged OnSelectedMinionChangedEvent;

        public delegate void OnMassChanged(int mass);
        public static event OnMassChanged OnMassChangedEvent;


        private CircleRenderer circleRenderer;
#nullable enable
        private Mass? grabbedMass = null;
        private int baseFOV = 50;

        void OnEnable()
        {
            MassChanged();
        }

        void Start()
        {
            SelectMinion(selectMinion);
            // create tongue rendere
            circleRenderer = GetComponent<CircleRenderer>();
            if (tongueRenderer == null) tongueRenderer = gameObject.AddComponent<LineRenderer>();
            tongueRenderer.startColor = Color.red;
            tongueRenderer.endColor = Color.red;
            // set width 0.1
            tongueRenderer.startWidth = 0.1f;
            tongueRenderer.endWidth = 0.05f;

            if (circleRenderer)
            {
                circleRenderer.Radius = baseMaxTongueLength;
            }

            MassChanged();
        }

        public void SpawnDoge()
        {
            SpawnManager.Instance.SpawnEntity(dogePrefab, transform.position);
        }

        public void IncreaseTongueLength(float length)
        {
            baseMaxTongueLength += length;
            if (circleRenderer)
            {
                circleRenderer.Radius += length;
            }
        }

        void MassChanged()
        {
            MaxHealth = 1 + (mass * healthPerMass);
            SetHealth(mass);

            var clampedMass = Math.Clamp(mass, 0, maxSize);

            GetComponent<Rigidbody2D>().mass = (float)(0.5 + (clampedMass * rbMassPerFly));
            var scale = 1 + (clampedMass * sizePerMass);
            transform.localScale = Vector3.one * scale;

            // Update cinemachine base follow offset z
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            var camera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (camera == null) return;
            camera.m_Lens.FieldOfView = baseFOV + (clampedMass * cameraZoomOutPerMass);

            OnMassChangedEvent.Invoke(mass);
        }

        public override void Damage(float amount)
        {
            base.Damage(amount);
            SetMass((int)GetHealth());
        }

        public void SetMass(int amount)
        {
            mass = amount;
            MassChanged();
        }

        public void IncreaseMass(int amount)
        {
            mass += amount;
            MassChanged();
        }

        public void DecreaseMass(int amount)
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
            if (finalMass < 1) return false;

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
                OnSelectedMinionChangedEvent?.Invoke(minions[selectMinion]);
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
                // spawnPos.y += spawnYDisplacement; // Displace spawn position below mouth

                GameObject lilGuy = SpawnManager.Instance.SpawnEntity(minionType.prefab, spawnPos);

                // pass minion count listner to the created minion
                Minion lilMinion = lilGuy.GetComponent<Minion>();

                return true;
            }
            else return false;
        }

        public void TongueGrab()
        {

            // find objects in physics 2D overlap circle
            var colliders = Physics2D.OverlapCircleAll(transform.position, baseMaxTongueLength * transform.localScale.x);

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

            IncreaseMass(grabbedMass.mass + AdditionalMassPerFly);
            Destroy(grabbedMass.gameObject);

            grabbedMass = null;
        }

        void Update()
        {

            if (grabbedMass != null)
            {
                tongueRenderer.enabled = true;
                mouthSprite.SetActive(true);
                var start = tongueRenderer.transform.position;
                var end = grabbedMass.transform.position;
                tongueRenderer.SetPositions(new Vector3[] { start, end });

                // Calculate the distance between the player and the minion
                float distance = Vector3.Distance(start, end);

                // If the minion is further away than the max tongue length, pull it closer
                if (distance > (baseMaxTongueLength * transform.localScale.x))
                {
                    // Move the minion towards the player
                    grabbedMass.transform.position = Vector3.MoveTowards(end, start, distance - baseMaxTongueLength);
                }
            }
            else
            {
                tongueRenderer.enabled = false;
                mouthSprite.SetActive(false);
            }
        }
    }
}