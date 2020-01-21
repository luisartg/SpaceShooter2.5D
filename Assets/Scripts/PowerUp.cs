using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _top = 7f;
    [SerializeField] float _bottomLimit = -7f;
    [SerializeField] float _leftLimit = -11f;
    [SerializeField] float _rightLimit = 11f;

    [SerializeField] int powerUpID = 0;

    [SerializeField] AudioClip _powerUpSound = null;

    private void Awake()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(UnityEngine.Random.Range(_leftLimit, _rightLimit), _top);
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < _bottomLimit)
        {
            Destroy(gameObject);
        }
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
        }
        
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
}
