using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _container = default;
    [SerializeField] private GameObject monstrePrefab;
    [SerializeField] private float spawnTime = 3.5f; // Temps entre chaque spawn
    [SerializeField] private float spawnRadius = 20f; // Le rayon de la zone de spawn autour du joueur

    private Transform playerTransform; // Référence au transform du joueur

    private bool _StopSpawning = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnEnemy(spawnTime, monstrePrefab));
    }

    private IEnumerator SpawnEnemy(float spawnTime, GameObject enemy)
    {
        while (!_StopSpawning)
        {
            yield return new WaitForSeconds(spawnTime);

            Vector3 spawnPosition = GetValidSpawnPosition();

            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _container.transform;
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 1f); // Vérifier les colliders autour de la position de spawn

        // Réessayer avec une nouvelle position si la position est invalide
        while (colliders.Length > 0)
        {
            randomOffset = Random.insideUnitCircle * spawnRadius;
            spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            colliders = Physics2D.OverlapCircleAll(spawnPosition, 1f);
        }

        return spawnPosition;
    }

    public void OnPlayerDeath()
    {
        _StopSpawning = true;
        Destroy(_container);
    }
}
