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
    private List<SpawnDef> validSpawnDefs = new List<SpawnDef>();

    // Pool for each entity type
    private Dictionary<GameObject, ObjectPool> entityPools = new Dictionary<GameObject, ObjectPool>();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // Initialize spawn timers and validSpawnDefs list
        foreach (var spec in spawnSpec)
        {
            foreach (var setting in spec.spawnSettings)
            {
                spawnTimers[setting] = 0f;
                if (!entityPools.ContainsKey(setting.prefab))
                {
                    var pool = new ObjectPool(setting.prefab, 10); // Adjust initial size as needed
                    entityPools.Add(setting.prefab, pool);
                }
            }
            validSpawnDefs.Add(spec);
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
                UpdateValidSpawnDefs(currentRound);
                SpawnAtRoundStart(currentRound);
                lastRound = currentRound;
            }

            foreach (var spec in validSpawnDefs)
            {
                foreach (var setting in spec.spawnSettings)
                {
                    if (setting.triggerSpawnEvery > 0)
                    {
                        spawnTimers[setting] += Time.deltaTime;
                        if (spawnTimers[setting] >= setting.triggerSpawnEvery)
                        {
                            SpawnEntitiesAtClosestOffScreenPoint(setting.prefab);
                            spawnTimers[setting] = 0f;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    private void UpdateValidSpawnDefs(float currentRound)
    {
        validSpawnDefs.Clear();
        foreach (var spec in spawnSpec)
        {
            if ((spec.minRound == 0 || currentRound >= spec.minRound) &&
                (spec.maxRound == 0 || currentRound <= spec.maxRound))
            {
                validSpawnDefs.Add(spec);
            }
        }
    }

    private void SpawnAtRoundStart(float currentRound)
    {
        foreach (var spec in validSpawnDefs)
        {
            foreach (var setting in spec.spawnSettings)
            {
                if (setting.triggerSpawnEvery == 0)
                {
                    SpawnEntitiesAtClosestOffScreenPoint(setting.prefab);
                }
            }
        }
    }

    private void SpawnEntitiesAtClosestOffScreenPoint(GameObject prefab)
    {
        GameObject closestSpawnPoint = null;
        float closestDistance = float.MaxValue;

        var playerPosition = PlayerController.Instance.transform.position; // Assuming you have a PlayerController

        foreach (var point in spawnPoints)
        {
            if (IsOffScreen(point))
            {
                float distance = Vector3.Distance(playerPosition, point.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSpawnPoint = point;
                }
            }
        }

        if (closestSpawnPoint != null)
        {
            SpawnEntity(prefab, closestSpawnPoint.transform.position);
        }
    }
    private bool IsOffScreen(GameObject point)
    {
        Vector2 viewportPos = mainCamera.WorldToViewportPoint(point.transform.position);
        return viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;
    }

    // Method to spawn an entity
    public GameObject SpawnEntity(GameObject entityPrefab, Vector3 position)
    {
        if (!entityPools.ContainsKey(entityPrefab))
        {
            var pool = new ObjectPool(entityPrefab, 10); // Adjust size as needed
            entityPools[entityPrefab] = pool;
        }

        var entity = entityPools[entityPrefab].Get();
        entity.transform.position = position;
        entity.transform.rotation = Quaternion.identity;

        var entityComponent = entity.GetComponent<Entity>();
        if (entityComponent != null)
        {
            entityComponent.InitializeEntity();
        }

        return entity;
    }

    // Method to despawn an entity
    public void DespawnEntity(GameObject entity)
    {
        var entityComponent = entity.GetComponent<Entity>();
        if (entityComponent != null)
        {
            entityComponent.ResetEntity();
        }

        // Find the correct pool to return the entity to
        foreach (var poolEntry in entityPools)
        {
            if (poolEntry.Value.GetPrefab(entity) == poolEntry.Key)
            {
                poolEntry.Value.Return(entity);
                break;
            }
        }
    }
}
