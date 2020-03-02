using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _directedSpeedMultiplier = 3f;
    [SerializeField] float _top = 7f;
    [SerializeField] float _bottomLimit = -7f;
    [SerializeField] float _leftLimit = -9f;
    [SerializeField] float _rightLimit = 9f;
    

    [SerializeField] int powerUpID = 0;

    [SerializeField] AudioClip _powerUpSound = null;
    Player _playerRef = null;

    private void Awake()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(UnityEngine.Random.Range(_leftLimit, _rightLimit), _top);
        GetComponent<Collider2D>().enabled = true;
        _playerRef = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (transform.position.y < _bottomLimit)
        {
            Destroy(gameObject);
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.Translate(GetDirectionToPlayer() * _speed * _directedSpeedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }
        
    }

    private Vector2 GetDirectionToPlayer()
    {
        return new Vector2(_playerRef.transform.position.x - transform.position.x,
                           _playerRef.transform.position.y - transform.position.y)
                           .normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        AudioSource.PlayClipAtPoint(_powerUpSound, Camera.main.transform.position);
        switch (powerUpID)
        {
            case 0: ActivateTripleShot(collision);
                break;
            case 1: ActivateSpeed(collision);
                break;
            case 2: ActivateShield(collision);
                break;
            case 3: AddAmmo(collision);
                break;
            case 4: AddLife(collision);
                break;
            case 5: ActivateEnergyShot(collision);
                break;
            case 6: ActivateSlowDown(collision);
                break;
            case 7: ActivateMissiles(collision);
                break;
        }
        
    }

    private void ActivateEnergyShot(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.ActivateEnergyShot();
        Destroy(gameObject);
    }

    private void AddLife(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.AddLife();
        Destroy(gameObject);
    }

    private void AddAmmo(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.AddAmmo();
        Destroy(gameObject);
    }

    private void ActivateShield(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.ActivateShield(true);
        Destroy(gameObject);
    }

    private void ActivateTripleShot(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.ActivateTripleShot();
        Destroy(gameObject);
    }

    private void ActivateSpeed(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.ActivateSpeedBoost();
        Destroy(gameObject);
    }

    private void ActivateSlowDown(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.ActivateSlowDown();
        Destroy(gameObject);
    }

    private void ActivateMissiles(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.ActivateMissiles();
        Destroy(gameObject);
    }
}
