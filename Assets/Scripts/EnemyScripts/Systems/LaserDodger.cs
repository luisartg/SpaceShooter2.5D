using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDodger : MonoBehaviour
{
    [SerializeField] float _radarDistance = 4.0f;
    [SerializeField] float _radarGap = 1.0f;
    GameObject _watchedProjectile = null;
    float _wpDistance = 0;
    bool _isDodging = false;

    EnemyCore _enemyCore;
    EnemyMovement _enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        _enemyCore = GetComponent<EnemyCore>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enemyCore.IsAlive()) return;

        if (_isDodging)
        {
            if (IsProjectileFarEnough())
            {
                RestartSystem();
            }
        }
        else
        {
            if (!_watchedProjectile)
            {
                FindNearestPlayerProjectiles();
            }

            if (_watchedProjectile)
            {
                MoveAway();
            }
        }
    }

    private bool IsProjectileFarEnough()
    {
        if (_watchedProjectile)
        {
            return (GetDistanceTo(_watchedProjectile) >= _radarDistance + _radarGap);
        }
        else
        {
            return true;
        }
        
    }

    private void RestartSystem()
    {
        _isDodging = false;
        _enemyMovement.RemoveCustomDirection();
        _watchedProjectile = null;
        _wpDistance = 0;
    }

    private void FindNearestPlayerProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Laser");
        float currentDistance = 0;
        foreach (var projectile in projectiles)
        {
            currentDistance = GetDistanceTo(projectile);
            if (currentDistance < _wpDistance || _watchedProjectile == null)
            {
                _watchedProjectile = projectile;
                _wpDistance = currentDistance;
            }
        }

        if (_wpDistance > _radarDistance)
        {
            // We only take projectiles that are inside detection radius.
            _watchedProjectile = null;
            _wpDistance = 0;
        }
    }

    private float GetDistanceTo(GameObject projectile)
    {
        return Mathf.Sqrt(Mathf.Pow(projectile.transform.position.x - transform.position.x, 2)
                        + Mathf.Pow(projectile.transform.position.y - transform.position.y, 2));
    }

    private void MoveAway()
    {
        if (_enemyCore.IsAlive())
        {
            _enemyMovement.SetCustomDirection(GetEscapeDirection());
            _isDodging = true;
        }
    }

    private Vector2 GetEscapeDirection()
    {
        Vector2 newDirection;
        float x = _watchedProjectile.transform.position.x - transform.position.x;
        if (x >= 0) //projectile is at the right
        {
            newDirection = new Vector2(-1, -1);
        }
        else
        {
            newDirection = new Vector2(1, -1);
        }
        return newDirection.normalized;
    }
}
