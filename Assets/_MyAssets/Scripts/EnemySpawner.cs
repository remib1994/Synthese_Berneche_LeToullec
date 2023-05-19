using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _container = default;
    [SerializeField] private GameObject monstrePrefab;
    [SerializeField] private float spawnTime = 3.5f;

    private Transform playerTransform; // Référence au transform du joueur

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnEnemy(spawnTime, monstrePrefab));
    }

    private IEnumerator SpawnEnemy(float spawnTime, GameObject enemy)
    {
        yield return new WaitForSeconds(spawnTime);

        // Calculer la position de spawn autour du joueur
        float spawnRadius = 20f; // Le rayon de la zone de spawn autour du joueur
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        StartCoroutine(SpawnEnemy(spawnTime, enemy));
        newEnemy.transform.parent = _container.transform;
    }
}
