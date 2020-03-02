using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeeker : MonoBehaviour
{
    [SerializeField] float _radarSize = 9;
    [SerializeField] float _radarGap = 1;
    [SerializeField] float _rotationSpeed = 1;

    EnemyCore _enemyLocked = null;
    MissileMovement _projectileMovement = null;
    Vector2 _distanceVector;
    Vector2 _directionVector;
    float _distanceToTraget = 0;


    private void Start()
    {
        _projectileMovement = GetComponent<MissileMovement>();
        _directionVector = Vector2.up;
    }

    private void Update()
    {
        if (_projectileMovement)
        {
            CalculateDistanceVector();
            CheckEnemyOnRadar();
            MoveToEnemy();
        }
    }

    private void CalculateDistanceVector()
    {
        if (!_enemyLocked || !_enemyLocked.IsAlive())
        {
            SearchNearestEnemy();
        }

        if (_enemyLocked && _enemyLocked.IsAlive())
        {
            _distanceVector = new Vector2(_enemyLocked.transform.position.x - transform.position.x,
                                         _enemyLocked.transform.position.y - transform.position.y);
        }
        else
        {
            //if enemy stops being alive, create a fake distance of twice the max detection distance, thus turning off the seeker
            _distanceVector = new Vector2((_radarSize + _radarGap) * 2, 0);
        }
    }

    private void SearchNearestEnemy()
    {
        EnemyCore[] enemyArray = FindObjectsOfType<EnemyCore>();
        float currentDistance = 0;
        foreach (EnemyCore enemy in enemyArray)
        {
            if (!enemy.IsAlive()) continue;

            currentDistance = GetDistanceTo(enemy);
            if (currentDistance < _distanceToTraget || _enemyLocked == null)
            {
                _enemyLocked = enemy;
                _distanceToTraget = currentDistance;
            }
        }
    }

    private float GetDistanceTo(EnemyCore enemy)
    {
        return Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2)
                        + Mathf.Pow(enemy.transform.position.y - transform.position.y, 2));
    }

    private void MoveToEnemy()
    {
        CalculateDirection();
        if (_projectileMovement)
        {
            _projectileMovement.SetDirection(_directionVector.normalized);
        }
    }

    private void CalculateDirection()
    {
        float current, target;
        current = GetAngleFrom(_directionVector);
        target = GetAngleFrom(_distanceVector);
        float gap = target - current;
        if (Mathf.Abs(gap) > 180) gap = -gap;
        float advancement = current + (Mathf.Sign(gap) * _rotationSpeed * Time.deltaTime);
        if (Mathf.Abs(gap) < _rotationSpeed * Time.deltaTime)
        {
            _directionVector = _distanceVector;
        }
        else
        {
            _directionVector = new Vector2(Mathf.Cos(advancement * Mathf.Deg2Rad), Mathf.Sin(advancement * Mathf.Deg2Rad));
        }
    }

    private float GetAngleFrom(Vector2 vector)
    {
        Vector2 normalized = vector.normalized;
        float angle = Mathf.Acos(normalized.x) * Mathf.Rad2Deg;
        if (normalized.y < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    private void CheckEnemyOnRadar()
    {
        if (_distanceVector.magnitude > _radarSize + _radarGap)
        {
            _distanceVector = Vector2.up;
        }
    }
}
