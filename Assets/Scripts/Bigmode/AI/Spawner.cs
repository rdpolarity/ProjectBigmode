using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bigmode
{
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private float spawnRadius = 2f;
        [SerializeField] private GameObject[] spawnableObjects = new GameObject[0];
        [SerializeField] private float spawnInterval = 10f;
        [SerializeField] private float spawnIntervalPerRound = 0.1f;
        [SerializeField] private float triggerRadius = 5f;
        [SerializeField] private string spawnTriggerTag = "Player";
        [SerializeField] private bool preventOnScreenSpawning = true;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        private void Start()
        {
            StartCoroutine(Spawning());
        }

        bool CheckIfTagInRange(string tag)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, triggerRadius);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag(tag))
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckIfOnScreen()
        {
            var screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }

        private IEnumerator Spawning()
        {
            while (true)
            {
                if (CheckIfTagInRange(spawnTriggerTag) && (!preventOnScreenSpawning || !CheckIfOnScreen()))
                {
                    Vector2 spawnPosition = (Vector2)(transform.position + Random.insideUnitSphere * spawnRadius);
                    var spawnObject = spawnableObjects[Random.Range(0, spawnableObjects.Length)];
                    Instantiate(spawnObject, spawnPosition, Quaternion.identity);
                }

                var spawnCooldown = Mathf.Clamp(spawnInterval - RoundManager.Instance.Round * spawnIntervalPerRound, 1, 100f);

                yield return new WaitForSeconds(spawnCooldown);
            }
        }
    }
}