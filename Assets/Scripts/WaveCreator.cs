using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCreator : MonoBehaviour
{
    // Control the creation of a Wave
    // it has a pool of pickups, enemies and bosses
    // each wave contains
    // - list with enemies allowed
    // - list with pickups allowed 
    // each list has their spawn rates, frequency of spawn, and number of simultaneous spawns.

    // we can produce a random wave generation/ or we can provide a list of levels, with each level with tweaked data

    [SerializeField] GameObject[] EnemyPool = null;
    [SerializeField] GameObject[] PickUpPool = null;
    private List<WaveData> waveList;

    public WaveData CreateBasicWave()
    {
        WaveData wave = new WaveData();
        // Crea un wave con todos los enemies y todos los pickups, con misma probabilidad
        for (int i = 0; i < EnemyPool.Length; i++)
        {
            GameObjectSpawnData enemy = new GameObjectSpawnData();
            enemy.GameObjectReference = EnemyPool[i];
            enemy.ProbabilityWeight = 1;
            wave.Enemies.Add(enemy);
        }

        for (int i = 0; i < PickUpPool.Length; i++)
        {
            GameObjectSpawnData pickup = new GameObjectSpawnData();
            pickup.GameObjectReference = PickUpPool[i];
            pickup.ProbabilityWeight = 1;
            wave.Pickups.Add(pickup);
        }

        return wave;
    }

    public void AddReferencesToWave(WaveData templateWave)
    {
        for (int i = 0; i < templateWave.Enemies.Count; i++)
        {
            templateWave.Enemies[i].GameObjectReference = EnemyPool[templateWave.Enemies[i].Index];
        }

        for (int i = 0; i < templateWave.Pickups.Count; i++)
        {
            templateWave.Pickups[i].GameObjectReference = PickUpPool[templateWave.Pickups[i].Index];
        }
    }

    public WaveData CreateRandomWave(int enemiesQuantity, 
                                     int pickupQuantity,
                                     Range enemyProbabilityRange, 
                                     Range pickupProbabilityRange, 
                                     Range enemySpawnCycleRange, 
                                     Range pickupSpawnCycleRange)
    {
        var randomWave = new WaveData();

        List<int> selection = GetUniqueRandomNumbers(EnemyPool.Length, enemiesQuantity);
        for (int i = 0; i < selection.Count; i++)
        {
            var enemyData = new GameObjectSpawnData();
            enemyData.Index = selection[i];
            enemyData.GameObjectReference = EnemyPool[selection[i]];
            enemyData.ProbabilityWeight = UnityEngine.Random.Range(enemyProbabilityRange.Min, enemyProbabilityRange.Max + 1);
            randomWave.Enemies.Add(enemyData);
        }

        selection = GetUniqueRandomNumbers(PickUpPool.Length, pickupQuantity);
        for (int i = 0; i < selection.Count; i++)
        {
            var pickupData = new GameObjectSpawnData();
            pickupData.Index = selection[i];
            pickupData.GameObjectReference = PickUpPool[selection[i]];
            pickupData.ProbabilityWeight = UnityEngine.Random.Range(pickupProbabilityRange.Min, pickupProbabilityRange.Max + 1);
            randomWave.Pickups.Add(pickupData);
        }

        randomWave.EnemySpawnCycle = enemySpawnCycleRange;
        randomWave.PickupSpawnCycle = pickupSpawnCycleRange;

        return randomWave;
    }

    private List<int> GetUniqueRandomNumbers(int length, int quantity)
    {
        List<int> randoms = new List<int>();
        List<int> series = new List<int>();
        for (int i = 0; i < length; i++)
        {
            series.Add(i);
        }

        for (int i = 0; i < quantity; i++)
        {
            int selection = UnityEngine.Random.Range(0, series.Count);
            randoms.Add(series[selection]);
            series.RemoveAt(selection);
        }

        return randoms;
    }
}

public class WaveData
{
    public List<GameObjectSpawnData> Enemies;
    public List<GameObjectSpawnData> Pickups;
    public Range EnemySpawnCycle;
    public Range PickupSpawnCycle;
    public int WaveDurationInSeconds;
    public int EnemiesPerCycle;
    public int PickupsPerCycle;

    private int _totalEnemyWeight;
    private int _totalPickupWeight;
    

    public WaveData()
    {
        Enemies = new List<GameObjectSpawnData>();
        Pickups = new List<GameObjectSpawnData>();
        EnemySpawnCycle = new Range();
        PickupSpawnCycle = new Range();
        WaveDurationInSeconds = 30;
        EnemiesPerCycle = 1;
        PickupsPerCycle = 1;
    }

    private int CalculateAccumulatedWeights(List<GameObjectSpawnData> items)
    {
        int total = 0;
        for (int i = 0; i < items.Count; i++)
        {
            total += items[i].ProbabilityWeight;
            items[i].AccumulatedWeight = total;
        }
        return total;
    }

    public void CalculateAllProbabilityWeights()
    {
        _totalEnemyWeight = CalculateAccumulatedWeights(Enemies);
        _totalPickupWeight = CalculateAccumulatedWeights(Pickups);
    }

    public GameObject GetRandomEnemy()
    {
        return Enemies[UnityEngine.Random.Range(0, Enemies.Count)].GameObjectReference;
    }

    public GameObject GetRandomPickup()
    {
        return Pickups[UnityEngine.Random.Range(0, Pickups.Count)].GameObjectReference;
    }

    public GameObject GetRandomEnemyByWeight()
    {
        GameObject result = null;
        int weightIndex = UnityEngine.Random.Range(0, _totalEnemyWeight + 1);
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (weightIndex <= Enemies[i].AccumulatedWeight)
            {
                result = Enemies[i].GameObjectReference;
                break;
            }
        }
        return result;
    }

    public GameObject GetRandomPickupByWeight()
    {
        GameObject result = null;
        int weightIndex = UnityEngine.Random.Range(0, _totalPickupWeight + 1);
        for (int i = 0; i < Pickups.Count; i++)
        {
            if (weightIndex <= Pickups[i].AccumulatedWeight)
            {
                result = Pickups[i].GameObjectReference;
                break;
            }
        }
        return result;
    }
}

public class GameObjectSpawnData
{
    private int _probabilityWeight = 1;

    public int Index = 0;
    public GameObject GameObjectReference = null;
    public int AccumulatedWeight;
    public int ProbabilityWeight
    {
        get => _probabilityWeight;
        set
        {
            if (value <= 0) { throw new System.Exception("Probability weight must be 1 at minimum"); }
            else { _probabilityWeight = value; }
        }
    }
}

public class Range
{
    public int Max = 0;
    public int Min = 0;
}
