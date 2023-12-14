using System.Collections;
using UnityEngine;

public class BiggieSmalls : MonoBehaviour
{
    [SerializeField] private float cooldown = 20f;
    [SerializeField] private GameObject flyPrefab;
    private GameObject flyInstance;
    private bool isCooldown = false;

    private void Start()
    {
        SpawnFly();
    }

    private void Update()
    {
        // Check if the fly is taken and cooldown is not already started
        if (flyInstance == null && !isCooldown)
        {
            StartCoroutine(StartCooldown());
        }
    }

    private void SpawnFly()
    {
        Vector2 spawnPosition = (Vector2)transform.position + new Vector2(0, 0.5f);
        flyInstance = Instantiate(flyPrefab, spawnPosition, Quaternion.identity);
        flyInstance.transform.SetParent(transform);
    }


    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        SpawnFly();
        isCooldown = false;
    }
}
