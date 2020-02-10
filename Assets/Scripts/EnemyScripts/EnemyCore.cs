using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [SerializeField] int _enemyScoreValue = 10;
    [SerializeField] float _shieldProbability = 0.2f;

    private bool _enemyAlive = true;
    private Player _playerRef = null;
    private Collider2D _collider = null;
    private EnemyShield _shield = null;

    private void Awake()
    {
        _playerRef = FindObjectOfType<Player>();
        _collider = GetComponent<Collider2D>();
        _shield = transform.Find("EnemyShield").gameObject.GetComponent<EnemyShield>();
    }

    private void Start()
    {
        CheckIfShieldWillBeActive();
    }

    private void CheckIfShieldWillBeActive()
    {
        if (_shield)
        {
            float prob = UnityEngine.Random.Range(0, 1.0f);
            if (prob <= _shieldProbability)
            {
                _shield.SetShield(true);
            }
        }
    }

    public bool IsAlive()
    {
        return _enemyAlive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string collisionTag = other.gameObject.tag;

        CheckLaserCollision(collisionTag, other);
        CheckPlayerCollision(collisionTag, other);
    }

    private void CheckLaserCollision(string collisionTag, Collider2D other)
    {
        if (collisionTag == "Laser")
        {
            Destroy(other.gameObject);
            if (!IsShieldProtecting())
            {
                FindObjectOfType<Player>().AddToScore(_enemyScoreValue);
                StartDestruction();
            }
        }

        if (collisionTag == "Energy")
        {
            if (!IsShieldProtecting())
            {
                FindObjectOfType<Player>().AddToScore(_enemyScoreValue);
                StartDestruction();
            }
        }
    }

    private bool IsShieldProtecting()
    {
        bool isProtected = false;
        if (_shield)
        {
            if (_shield.GetComponent<EnemyShield>().IsShieldActive())
            {
                isProtected = true;
                _shield.SetShield(false);
            }
            else
            {
                isProtected = false;
            }
        }
        else
        {
            isProtected = false;
        }
        return isProtected;
    }

    private void CheckPlayerCollision(string collisionTag, Collider2D other)
    {
        if (collisionTag == "Player")
        {
            Debug.Log("Collision with player!");
            _playerRef.Damage();
            if (!IsShieldProtecting())
            {
                StartDestruction();
            }
        }
    }

    private void StartDestruction()
    {
        _enemyAlive = false;
        _collider.enabled = false;
        transform.Find("Explosion").gameObject.SetActive(true);
        StartCoroutine(RemoveShipGraphic());
        Destroy(gameObject, 3);
    }

    IEnumerator RemoveShipGraphic()
    {
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
