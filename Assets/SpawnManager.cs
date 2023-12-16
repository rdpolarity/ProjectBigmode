using System.Collections;
using System.Collections.Generic;
using Bigmode;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject[] spawnPoints;
    public SpawnDef[] spawnSpec;
    private Dictionary<SpawnDef.SpawnSettings, float> spawnTimers = new Dictionary<SpawnDef.SpawnSettings, float>();

    private float lastRound = -1;

    void Start()
    {
        // Initialize spawn timers for each setting
        foreach (var spec in spawnSpec)
        {
            foreach (var setting in spec.spawnSettings)
            {
                spawnTimers[setting] = 0f;
            }
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            var currentRound = RoundManager.Instance.Round;
            if (currentRound != lastRound)
            {
                SpawnAtRoundStart(currentRound);
                lastRound = currentRound;
            }

            foreach (var spec in spawnSpec)
            {
                if ((spec.minRound == 0 || currentRound >= spec.minRound) &&
                    (spec.maxRound == 0 || currentRound <= spec.maxRound))
                {
                    foreach (var setting in spec.spawnSettings)
                    {
                        if (setting.triggerSpawnEvery > 0)
                        {
                            spawnTimers[setting] += Time.deltaTime;
                            if (spawnTimers[setting] >= setting.triggerSpawnEvery)
                            {
                                SpawnEnemiesAtClosestOffScreenPoint(setting.prefab);
                                spawnTimers[setting] = 0f; // Reset timer
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }

    private void SpawnAtRoundStart(float currentRound)
    {
        foreach (var spec in spawnSpec)
        {
            if ((spec.minRound == 0 || currentRound >= spec.minRound) &&
                (spec.maxRound == 0 || currentRound <= spec.maxRound))
            {
                foreach (var setting in spec.spawnSettings)
                {
                    if (setting.triggerSpawnEvery == 0)
                    {
                        SpawnEnemiesAtClosestOffScreenPoint(setting.prefab);
                    }
                }
            }
        }
    }

    private void SpawnEnemiesAtClosestOffScreenPoint(GameObject enemyPrefab)
    {
        var orderedSpawnPoints = GetOrderedSpawnPointsByDistance();
        foreach (var point in orderedSpawnPoints)
        {
            if (IsOffScreen(point))
            {
                Instantiate(enemyPrefab, point.transform.position, Quaternion.identity);
                break;
            }
        }
    }

    private bool IsOffScreen(GameObject point)
    {
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(point.transform.position);
        return viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;
    }


    private GameObject[] GetOrderedSpawnPointsByDistance()
    {
        var playerPosition = PlayerController.Instance.transform.position;
        if (playerPosition == null) return spawnPoints;

        var spawnPointsByDistance = new List<GameObject>(spawnPoints);
        spawnPointsByDistance.Sort((a, b) =>
        {
            var distanceA = Vector3.Distance(a.transform.position, playerPosition);
            var distanceB = Vector3.Distance(b.transform.position, playerPosition);
            return distanceA.CompareTo(distanceB);
        });

        return spawnPointsByDistance.ToArray();
    }
}
