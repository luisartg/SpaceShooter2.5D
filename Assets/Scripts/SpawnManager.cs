using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyContainer = null;
    WaveDirector _waveDirector;
    WaveData _currentWave;

    List<int> _enemiesWeights;
    List<int> _pickupsWeights;

    bool _continueSpawning;
    bool _continuePickups;
    bool _pickupsStopped;
    int _waveCount;

    UIManager _uiManager = null;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _waveDirector = GetComponent<WaveDirector>();
        _waveCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        _waveCount++;
        _uiManager.ShowLevel(_waveCount);
        _currentWave = _waveDirector.GetWave();
        
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(StartWaveTimeLimit());
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        _continueSpawning = true;
        yield return new WaitForSeconds(3);
        while (_continueSpawning)
        {
            for (int i = 0; i < _currentWave.EnemiesPerCycle; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(_currentWave.EnemySpawnCycle.Min, _currentWave.EnemySpawnCycle.Max));
        }
        Debug.Log("Spawner finished");
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(_currentWave.GetRandomEnemyByWeight());
        enemy.transform.parent = _enemyContainer.transform;
        //enemy.ChangeMovementTypeTo(UnityEngine.Random.Range(0, 2));
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        _continuePickups = true;
        _pickupsStopped = false;
        yield return new WaitForSeconds(3);
        while (_continuePickups)
        {
            GameObject powerUp = Instantiate(_currentWave.GetRandomPickupByWeight());
            yield return new WaitForSeconds(UnityEngine.Random.Range(_currentWave.PickupSpawnCycle.Min, _currentWave.PickupSpawnCycle.Min));
        }
        _pickupsStopped = true;
    }

    IEnumerator StartWaveTimeLimit()
    {
        yield return new WaitForSeconds(_currentWave.WaveDurationInSeconds);
        StopSpawning(true);
    }


    public void StopSpawning(bool goNextWave = false)
    {
        _continueSpawning = false;

        if (goNextWave)
        {
            StartCoroutine(WaitUntilAllEnemiesDestroyed());
        }
        else
        {
            _continuePickups = false;
        }
    }

    IEnumerator WaitUntilAllEnemiesDestroyed()
    {
        bool allEnemiesDestroyed = false;
        while (!allEnemiesDestroyed)
        {
            if (FindObjectsOfType<EnemyCore>().Length == 0)
            {
                allEnemiesDestroyed = true;
                _continuePickups = false;
            }
            else
            {
                yield return new WaitForSeconds(2);
            }
        }

        while (!_pickupsStopped)
        {
            yield return new WaitForSeconds(1); 
            // we are making sure the pickups coroutine has stopped so a new one can start
        }

        StartSpawning();
    }
}

public class WeightItem
{
    public GameObject Item = null;
    public float Weight = 0f;
    public float CalculatedWeight = 0f;
}
