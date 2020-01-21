using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyContainer = null;
    [SerializeField] Enemy _enemyReference = null;
    [SerializeField] int _enemySpawnPeriodInSeconds = 3;

    [SerializeField] PowerUp[] powerUps = null;
    [SerializeField] int _puSpawnPeriodInSecondsMax = 7;
    [SerializeField] int _puSpawnPeriodInSecondsMin = 3;

   

    bool _continueSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);
        while (_continueSpawning)
        {
            Enemy enemy = Instantiate(_enemyReference);
            enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnPeriodInSeconds);
        }
        Debug.Log("Spawner finished");
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3);
        while (_continueSpawning)
        {
            PowerUp powerUp = Instantiate(powerUps[Random.Range(0, powerUps.Length)]);
            yield return new WaitForSeconds(Random.Range(_puSpawnPeriodInSecondsMin, _puSpawnPeriodInSecondsMax));
        }
    }

    public void StopSpawning()
    {
        _continueSpawning = false;
    }
}
