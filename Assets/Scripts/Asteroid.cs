using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float RotationSpeed = 3f;
    [SerializeField] GameObject ExplosionEffect = null;
    [SerializeField] bool _asteroidAlive = true;

    SpawnManager _spawnManager = null;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateAsteroid();
    }

    private void RotateAsteroid()
    {
        if (_asteroidAlive)
        {
            transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            GameObject explosion = Instantiate(ExplosionEffect, transform);
            explosion.GetComponent<SpriteRenderer>().sortingOrder = 1;
            StartCoroutine(RemoveAsteroid());
            GetComponent<Collider2D>().enabled = false;
            _spawnManager.StartSpawning();
            Destroy(gameObject, 3.1f);
        }
    }

    private IEnumerator RemoveAsteroid()
    {
        yield return new WaitForSeconds(1);
        _asteroidAlive = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
