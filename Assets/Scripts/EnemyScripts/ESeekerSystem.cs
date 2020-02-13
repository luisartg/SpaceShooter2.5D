using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESeekerSystem : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    [SerializeField] float _radarSize = 9;
    [SerializeField] float _radarGap = 1;
    Player _playerRef = null;
    EnemyMovement _enemyMovement = null;
    EnemyCore _enemyCore = null;
    bool _followingEnabled = false;
    Vector2 _distanceVector;


    private void Start()
    {
        _playerRef = FindObjectOfType<Player>();
        _enemyMovement = transform.parent.GetComponent<EnemyMovement>();
        _enemyCore = transform.parent.GetComponent<EnemyCore>();
    }

    private void Update()
    {
        if (_enemyCore.IsAlive())
        {
            CalculateDistanceVector();
            CheckPlayerOnRadar();
            if (_followingEnabled)
            {
                MoveToPlayer();
            }
        }
    }

    private void CalculateDistanceVector()
    {
        if (_playerRef)
        {
            _distanceVector = new Vector2(_playerRef.transform.position.x - transform.position.x,
                                         _playerRef.transform.position.y - transform.position.y);
        }
        else
        {
            //if player stops being alive, create a fake distance of twice the max detection distance, thus turning off the seeker
            _distanceVector = new Vector2((_radarSize + _radarGap) * 2, 0);
        }
    }

    private void MoveToPlayer()
    {
        transform.parent.transform.Translate(_distanceVector.normalized * _speed * Time.deltaTime);
    }

    private void CheckPlayerOnRadar()
    {
        Debug.Log(_distanceVector.magnitude.ToString());
        if (_distanceVector.magnitude > _radarSize + _radarGap)
        {
            EnableFollowing(false);
            Debug.Log("Following diabled!");
        }
        if (_distanceVector.magnitude < _radarSize)
        {
            EnableFollowing(true);
            Debug.Log("Following enabled!");
        }
    }

    private void EnableFollowing(bool enable)
    {
        if (_enemyMovement)
        {
            _enemyMovement.EnableMovement(!enable);
        }
        _followingEnabled = enable;
    }
}
