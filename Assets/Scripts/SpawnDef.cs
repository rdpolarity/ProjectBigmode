

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnDef", menuName = "Bigmode/SpawnDef", order = 0)]
public class SpawnDef : SerializedScriptableObject
{
    [InfoBox("The minimum round number from which this spawn definition becomes active. A value of 0 indicates that there's no minimum round requirement, making this spawn definition always active from the start.")]
    public int minRound = 0;
    [InfoBox("The maximum round number until which this spawn definition remains active. A value of 0 implies no maximum round limit, allowing this definition to remain active indefinitely.")]
    public int maxRound = 0;
    [InfoBox(@" 
    A list of SpawnSettings structures, each defining a specific spawning behavior. Each SpawnSettings includes:
        prefab (GameObject): The prefab of the game object to be spawned.
        triggerSpawnEvery (float): The interval in seconds at which to spawn the game object. A value of 0 indicates that the object should be spawned only once at the start of each round.
    ")]
    public List<SpawnSettings> spawnSettings = new();

    public struct SpawnSettings
    {
        public GameObject prefab;
        // 0 = once at the start of every round
        public float triggerSpawnEvery;
    }
}