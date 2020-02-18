using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EnemyScripts.Weapons;
using System;

public class ItemDestroyer : MonoBehaviour
{
    [SerializeField] float _arcDegrees = 10;
    [SerializeField] float _shootTime = 2f;
    PowerUp[] _powerUps = null;
    EnemyCore _enemyCore = null;
    IWeapon _weapon = null;
    bool _isPowerUpAtFront = false;
    float _shootCurrentTime = 0;
    bool isShotCharged = false;

    private void Start()
    {
        _enemyCore = GetComponent<EnemyCore>();
        _weapon = GetComponent<IWeapon>();
    }

    private void Update()
    {
        if (isShotCharged)
        {
            DetectPowerUpsInFront();
            ShootIfPowerUpsInFront();
        }
        else
        {
            isShotCharged = IsShotAvailable();
        }
    }

    private void DetectPowerUpsInFront()
    {
        _isPowerUpAtFront = false;
        _powerUps = FindObjectsOfType<PowerUp>();
        foreach (var powerup in _powerUps)
        {
            if (LineOfSightInsideArc(powerup.transform.position))
            {
                _isPowerUpAtFront = true;
                break;
            }
        }
    }

    private bool LineOfSightInsideArc(Vector2 powerUpPosition)
    {
        bool isAtFront = true;
        float posY = powerUpPosition.y - transform.position.y;
        if (posY < 0)
        {
            float posX = powerUpPosition.x - transform.position.x;
            Vector2 direction = new Vector2(posX, posY);
            direction.Normalize();
            float angle = Mathf.Acos(direction.x) * Mathf.Rad2Deg;
            if (angle <= 90 + (_arcDegrees / 2) && angle >= 90 - (_arcDegrees / 2))
            {
                isAtFront = true;
            }
        }
        return isAtFront;
    }

    private void ShootIfPowerUpsInFront()
    {
        if (_isPowerUpAtFront)
        {
            _weapon.Shoot(new Vector2(0, -1));
            isShotCharged = false;
        }
    }

    private bool IsShotAvailable()
    {
        _shootCurrentTime += Time.deltaTime;
        if (_shootCurrentTime >= _shootTime)
        {
            _shootCurrentTime = 0;
            return true;
        }
        return false;
    }
}
