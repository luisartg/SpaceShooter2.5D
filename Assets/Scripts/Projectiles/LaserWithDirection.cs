using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWithDirection : MonoBehaviour
{
    [SerializeField] float _speed = 8f;
    [SerializeField] float _topLimit = 8f;
    [SerializeField] float _bottomLimit = -8f;
    [SerializeField] float _leftLimit = -11f;
    [SerializeField] float _rightLimit = 11f;
    [SerializeField] Vector2 _direction = Vector2.down;

    // Update is called once per frame
    void Update()
    {
        Move();
        DestroyIfOutOfBounds();
    }

    private void Move()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
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
    }
}
