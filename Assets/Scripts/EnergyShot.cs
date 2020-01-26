using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShot : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] float _topLimit = 8f;
    [SerializeField] float _bottomLimit = -8f;
    [SerializeField] float _leftLimit = -11f;
    [SerializeField] float _rightLimit = 11f;
    [SerializeField] Vector2 _direction = Vector2.up;
    [SerializeField] int _currentEnergyPower = 3;

    private void Awake()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
        if (IsOutOfLimits())
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private bool IsOutOfLimits()
    {
        return transform.position.y > _topLimit
            || transform.position.y < _bottomLimit
            || transform.position.x < _leftLimit
            || transform.position.x > _rightLimit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            GetComponent<Collider2D>().enabled = false;
            if (_currentEnergyPower > 0)
            {
                MultiplyAttack(transform.position);
            }
            Destroy(gameObject);
        }
    }

    private void MultiplyAttack(Vector2 position)
    {
        for (int i = 0; i < 8; i++)
        {
            EnergyShot energyShot = Instantiate(this.gameObject, position, Quaternion.identity).GetComponent<EnergyShot>();
            energyShot.SetEnergyPower(_currentEnergyPower - 1);
            energyShot.SetDirection(GetDirectionFromAngle(0.785398f * i)); //0.785398 is 45º in radians 
        }
    }

    private Vector2 GetDirectionFromAngle(float angle)
    {
        Vector2 directionVector = new Vector2();
        directionVector.y = Mathf.Sin(angle);
        directionVector.x = Mathf.Cos(angle);
        return directionVector;
    }

    public void SetEnergyPower(int power)
    {
        _currentEnergyPower = power;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    
}
