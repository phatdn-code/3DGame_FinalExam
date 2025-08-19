using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float spawnSuccessRate = 0.5f; // 50% chance to spawn an enemy
    private Coroutine spawnCoroutine;

    private void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            // Check if the spawn is successful based on the success rate
            if (Random.value <= spawnSuccessRate)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
                GameManager.Instance.EnemySpawned(1);
            }
        }
    }

    public void StopSpawn() => StopCoroutine(spawnCoroutine);
}
