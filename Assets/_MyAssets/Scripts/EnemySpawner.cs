using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _container = default;
    [SerializeField] private GameObject[] monstrePrefab;
    [SerializeField] private float spawnTime = 3.5f; // Temps entre chaque spawn
    [SerializeField] private float spawnRadius = 20f; // Le rayon de la zone de spawn autour du joueur

    private Transform playerTransform; // R�f�rence au transform du joueur

    private UIManager _uiManager;

    private bool _StopSpawning = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnEnemy(spawnTime));
    }

    private IEnumerator SpawnEnemy(float spawnTime)
    {
        
        while (!_StopSpawning)
        {
            yield return new WaitForSeconds(spawnTime);
            Vector3 spawnPosition = GetValidSpawnPosition();

            if (_uiManager.getScore() < 1000)
            {
                GameObject newEnemy = Instantiate(monstrePrefab[0], spawnPosition, Quaternion.identity);
                newEnemy.transform.parent = _container.transform;
            }
            else if (_uiManager.getScore() < 2000)
            {
                int randomEnemy = Random.Range(0, 1);
                GameObject newEnemy = Instantiate(monstrePrefab[randomEnemy], spawnPosition, Quaternion.identity);
            }
            else
            {
                int randomEnemy = Random.Range(0, monstrePrefab.Length);
                GameObject newEnemy = Instantiate(monstrePrefab[randomEnemy], spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 1f); // V�rifier les colliders autour de la position de spawn

        // R�essayer avec une nouvelle position si la position est invalide
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
