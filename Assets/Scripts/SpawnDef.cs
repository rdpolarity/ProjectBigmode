

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnDef", menuName = "Bigmode/SpawnDef", order = 0)]
public class SpawnDef : SerializedScriptableObject
{
    // 0 = infinite
    public int minRound = 0;
    // 0 = infinite
    public int maxRound = 0;

    public List<SpawnSettings> spawnSettings = new();

    public struct SpawnSettings
    {
        public GameObject prefab;
        // 0 = once at the start of every round
        public float triggerSpawnEvery;
    }
}