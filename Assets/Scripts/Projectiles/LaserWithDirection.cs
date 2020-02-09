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

    GameObject laser = null;

    private void Awake()
    {
        laser = transform.Find("Laser").gameObject;
    }

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
        laser.transform.rotation = Quaternion.Euler(0,0,GetRotation(_direction));
    }

    private float GetRotation(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.x);
        if (direction.y < 0)
        {
            angle = 2 * Mathf.PI - angle;
        }
        return angle * 180 / Mathf.PI + 90; //we rotate additional 90º due to initial vertical position of the laser graphic
    }
}
