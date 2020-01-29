using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyContainer = null;
    [SerializeField] Enemy _enemyReference = null;
    [SerializeField] int _enemySpawnPeriodInSeconds = 3;

    [SerializeField] PowerUp[] _powerUpsArray = null;
    [SerializeField] float[] _powerUpsWeights = null;
    [SerializeField] List<WeightItem> _powerUps = null;
    [SerializeField] int _puSpawnPeriodInSecondsMax = 7;
    [SerializeField] int _puSpawnPeriodInSecondsMin = 3;

   

    bool _continueSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        ConstructPowerUpList();
    }

    private void ConstructPowerUpList()
    {
        _powerUps = new List<WeightItem>();
        float total = 0;
        for (int i = 0; i < _powerUpsArray.Length; i++)
        {
            var weightItem = new WeightItem();
            weightItem.Item = _powerUpsArray[i].gameObject;
            weightItem.Weight = _powerUpsWeights[i];
            total += weightItem.Weight;
            weightItem.CalculatedWeight = total;
            _powerUps.Add(weightItem);
        }
    }

    private GameObject GetItemFromWeight(float weightToSearch)
    {
        GameObject item = null;
        for (int i = _powerUps.Count-1; i >= 1; i--)
        {
            if (weightToSearch <= _powerUps[i].CalculatedWeight && weightToSearch > _powerUps[i-1].CalculatedWeight)
            {
                item = _powerUps[i].Item;
                break;
            }
        }
        //test
        if (!item)
        {
            item = _powerUps[0].Item;
        }
        return item;
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
            enemy.ChangeMovementTypeTo(UnityEngine.Random.Range(0, 2));
            yield return new WaitForSeconds(_enemySpawnPeriodInSeconds);
        }
        Debug.Log("Spawner finished");
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3);
        while (_continueSpawning)
        {
            GameObject powerUp = Instantiate(GetItemFromWeight(UnityEngine.Random.Range(0, _powerUps[_powerUps.Count-1].CalculatedWeight)));
            yield return new WaitForSeconds(UnityEngine.Random.Range(_puSpawnPeriodInSecondsMin, _puSpawnPeriodInSecondsMax));
        }
    }

    public void StopSpawning()
    {
        _continueSpawning = false;
    }
}

public class WeightItem
{
    public GameObject Item = null;
    public float Weight = 0f;
    public float CalculatedWeight = 0f;
}
