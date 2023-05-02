using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _EnemyPrefab = default;
    [SerializeField] private GameObject _EnemyContainer = default;
    [SerializeField] private float _VitesseSpawnEnemy = 2.5f;
    [SerializeField] private float _VitesseSpawnPU = 2f;
    [SerializeField] private GameObject[] _listePUPrefab = default;

    private bool _StopSpawning = false;
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPURoutine());
    }

    IEnumerator SpawnPURoutine()
    {
        yield return new WaitForSeconds(Random.Range(2.0f,4.0f));
        while (!_StopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.5f, 8.5f), 7f, 0f);
            int randomPU = Random.Range(0, _listePUPrefab.Length);
            Instantiate(_listePUPrefab[randomPU], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_VitesseSpawnPU);
        }
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        while (!_StopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.5f, 8.5f), 7f, 0f);
            GameObject newEnemy = Instantiate(_EnemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _EnemyContainer.transform;
            yield return new WaitForSeconds(_VitesseSpawnEnemy);
        }
    }

    public void OnPlayerDeath()
    {
        _StopSpawning = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
