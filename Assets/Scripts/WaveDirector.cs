using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDirector : MonoBehaviour
{
    [SerializeField] int _AddTimeEachNWaves = 2;
    [SerializeField] int _AddedWaveTime = 3;
    [SerializeField] int _AddEnemiesAtOnceEachNWaves = 10;
    [SerializeField] int _AddedEnemies = 1;
    [SerializeField] int _SpawnCycleInitialTime = 10;

    WaveCreator _waveCreator = null;
    List<WaveData> _levelWaves;
    WaveData _wave;
    GameObjectSpawnData _goSpawnData;

    [SerializeField] int _currentSpawnCyleTime;
    [SerializeField] int _currentEnemiesNumber;
    [SerializeField] int _currentWaveCount;
    [SerializeField] int _currentWaveDuration;

    private void Awake()
    {
        _currentWaveCount = 0;
        _currentEnemiesNumber = 1;
        _currentWaveDuration = 30;
        _waveCreator = GetComponent<WaveCreator>();
        CreateWaveSeries();
    }

    private void Start()
    {
        
    }

    public void CreateWaveSeries()
    {
        _levelWaves = new List<WaveData>();

        // wave 1
        _wave = new WaveData();
        _wave.Enemies.Add(GetGameObjectSpawnData(0,1));
        //pickups
        _wave.Pickups.Add(GetGameObjectSpawnData(0, 20)); // triple shot
        _wave.Pickups.Add(GetGameObjectSpawnData(1, 10)); // speed
        _wave.Pickups.Add(GetGameObjectSpawnData(2, 20)); // shield
        _wave.Pickups.Add(GetGameObjectSpawnData(3, 15)); // life
        _wave.Pickups.Add(GetGameObjectSpawnData(4, 5));  // energy shot
        _wave.Pickups.Add(GetGameObjectSpawnData(5, 30)); // ammo
        
        _wave.EnemySpawnCycle.Max = 3;
        _wave.EnemySpawnCycle.Min = 3;
        _wave.PickupSpawnCycle.Max = 6;
        _wave.PickupSpawnCycle.Min = 3;

        _waveCreator.AddReferencesToWave(_wave);
        _wave.CalculateAllProbabilityWeights();
        

        _levelWaves.Add(_wave);
    }

    public WaveData GetWave()
    {
        _wave = new WaveData();
        _currentWaveCount++;
        _wave.Enemies.Add(GetGameObjectSpawnData(0, 10));
        _wave.Enemies.Add(GetGameObjectSpawnData(1, 10));
        _wave.Enemies.Add(GetGameObjectSpawnData(2, 10));
        _wave.Enemies.Add(GetGameObjectSpawnData(3, 10));
        _wave.Enemies.Add(GetGameObjectSpawnData(4, 10));
        _wave.Enemies.Add(GetGameObjectSpawnData(5, 10));

        //pickups
        _wave.Pickups.Add(GetGameObjectSpawnData(0, 20)); // triple shot
        _wave.Pickups.Add(GetGameObjectSpawnData(1, 10)); // speed
        _wave.Pickups.Add(GetGameObjectSpawnData(2, 20)); // shield
        _wave.Pickups.Add(GetGameObjectSpawnData(3, 10)); // life
        _wave.Pickups.Add(GetGameObjectSpawnData(4, 5));  // energy shot
        _wave.Pickups.Add(GetGameObjectSpawnData(5, 30)); // ammo
        _wave.Pickups.Add(GetGameObjectSpawnData(6, 5)); // slowdown pickup



        _waveCreator.AddReferencesToWave(_wave);
        _wave.CalculateAllProbabilityWeights();
        CalculateEnemiesPerCycle();
        _wave.EnemiesPerCycle = _currentEnemiesNumber;
        _wave.EnemySpawnCycle = GetSpawnCycle();
        _wave.PickupSpawnCycle.Max = 6;
        _wave.PickupSpawnCycle.Min = 3;
        CalculateWaveDuration();
        _wave.WaveDurationInSeconds = _currentWaveDuration;
        return _wave;
    }

    private void CalculateWaveDuration()
    {
        if (_currentWaveCount % _AddTimeEachNWaves == 0)
        {
            _currentWaveDuration += _AddedWaveTime;
        }
    }

    private Range GetSpawnCycle()
    {
        int wavesInTheCycle = _currentWaveCount % _AddEnemiesAtOnceEachNWaves;
        int timeSubtraction = (int)Mathf.Ceil((float)(wavesInTheCycle * _SpawnCycleInitialTime) / (float)(_AddEnemiesAtOnceEachNWaves)) - 1;
        _currentSpawnCyleTime = _SpawnCycleInitialTime - timeSubtraction;
        var range = new Range();
        range.Min = _currentSpawnCyleTime - 1;
        range.Max = _currentSpawnCyleTime;
        if (range.Min <= 0) range.Min = 1;
        return range;
    }

    private void CalculateEnemiesPerCycle()
    {
        if (_currentWaveCount % _AddEnemiesAtOnceEachNWaves == 0)
        {
            _currentEnemiesNumber += _AddedEnemies;
        }
    }

    public int GetCurrentWaveIndex()
    {
        return _currentWaveCount;
    }

    private GameObjectSpawnData GetGameObjectSpawnData(int index, int weight)
    {
        GameObjectSpawnData g = new GameObjectSpawnData();
        g.Index = index;
        g.ProbabilityWeight = weight;
        return g;
    }
}
