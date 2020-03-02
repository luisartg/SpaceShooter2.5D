using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    [SerializeField] float _speed = 18f;
    [SerializeField] float _secondsToReachMaxSpeed = 1.5f;
    [SerializeField] float _topLimit = 8f;
    [SerializeField] float _bottomLimit = -8f;
    [SerializeField] float _leftLimit = -11f;
    [SerializeField] float _rightLimit = 11f;
    [SerializeField] Vector2 _direction;


    float _currentTime;
    float _currentSpeed;
    GameObject _body;

    // Start is called before the first frame update
    void Start()
    {
        _body = transform.Find("Body").gameObject;
        SetDirection(Vector2.up);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DestroyIfOutOfBounds();
    }

    private void Move()
    {
        transform.Translate(_direction * GetSpeed() * Time.deltaTime);
    }

    private float GetSpeed()
    {
        float speed = 0;
        _currentTime += Time.deltaTime;
        if (_currentTime > _secondsToReachMaxSpeed)
        {
            speed = _speed;
        }
        else
        {
            speed = _currentTime * _speed / _secondsToReachMaxSpeed;
        }
        return speed;
    }

    private void DestroyIfOutOfBounds()
    {
        if (IsOutOfBounds())
        {
            DestroyParentContainer();
            Destroy(gameObject);
        }
    }

    private bool IsOutOfBounds()
    {
        return transform.position.y > _topLimit ||
               transform.position.y < _bottomLimit ||
               transform.position.x > _rightLimit ||
               transform.position.x < _leftLimit;
    }

    private void DestroyParentContainer()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;
        if (_body)
        {
            _body.transform.rotation = Quaternion.Euler(0, 0, GetRotation(_direction));
        }
    }

    private float GetRotation(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.x);
        if (direction.y < 0)
        {
            angle = 2 * Mathf.PI - angle;
        }
        return angle * 180 / Mathf.PI; //we rotate additional 90º due to initial vertical position of the laser graphic
    }
}
