using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [SerializeField] int _enemyScoreValue = 10;

    private bool _enemyAlive = true;
    private Player _playerRef = null;
    private Collider2D _collider = null;

    private void Awake()
    {
        _playerRef = FindObjectOfType<Player>();
        _collider = GetComponent<Collider2D>();
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
            FindObjectOfType<Player>().AddToScore(_enemyScoreValue);
            Destroy(other.gameObject);
            StartDestruction();
        }

        if (collisionTag == "Energy")
        {
            FindObjectOfType<Player>().AddToScore(_enemyScoreValue);
            StartDestruction();
        }
    }

    private void CheckPlayerCollision(string collisionTag, Collider2D other)
    {
        if (collisionTag == "Player")
        {
            Debug.Log("Collision with player!");
            _playerRef.Damage();
            StartDestruction();
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
