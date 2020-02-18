using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EnemyScripts.Weapons;
using System;

public class BackWatcher : MonoBehaviour
{
    [SerializeField] float _arcDegrees = 120;
    [SerializeField] float _shootTime = 0.2f;
    IWeapon _weapon = null;
    Player _playerRef = null;
    EnemyCore _enemyCore = null;
    bool _isPlayerBehind = false;
    float _shootCurrentTime = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        _weapon = GetComponent<IWeapon>();
        _playerRef = FindObjectOfType<Player>();
        _enemyCore = GetComponent<EnemyCore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyCore.IsAlive())
        {
            CheckIfPlayerBehind();
            ShootIfPlayerBehind();
        }
    }

    private void CheckIfPlayerBehind()
    {
        if (!_playerRef)
        {
            _isPlayerBehind = false;
            return;
        }

        float posY = _playerRef.transform.position.y - transform.position.y;
        if (posY > 0)
        {
            float posX = _playerRef.transform.position.x - transform.position.x;
            Vector2 direction = new Vector2(posX, posY);
            direction.Normalize();
            float angle = Mathf.Acos(direction.x) * Mathf.Rad2Deg;
            if (angle <= 90 + (_arcDegrees / 2) && angle >= 90 - (_arcDegrees / 2))
            {
                _isPlayerBehind = true;
            }
            else
            {
                _isPlayerBehind = false;
            }
        }
        else
        {
            _isPlayerBehind = false;
        }

        if (!_isPlayerBehind) _shootCurrentTime = 0;
    }

    private void ShootIfPlayerBehind()
    {
        if (_isPlayerBehind && IsShotAvailable())
        {
            _weapon.Shoot(new Vector2(0,1));
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
