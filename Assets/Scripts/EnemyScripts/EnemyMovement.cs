using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Movement;
using System;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] float _top = 7f;
    [SerializeField] float _bottom = -7f;
    [SerializeField] float _right = 11f;
    [SerializeField] float _left = -11f;
    [SerializeField] Vector2 _currentMovement;
    [SerializeField] Vector2 _customDirection;
    [SerializeField] bool _movementEnabled = true;
    private IMovement _movementGenerator = null;

    private EnemyCore _enemyCore = null;

    private void Awake()
    {
        RemoveCustomDirection();
        ResetPosition();
        _enemyCore = GetComponent<EnemyCore>();
        _movementGenerator = new StraightDownMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyCore.IsAlive())
        {
            GenerateMovement();
            CheckEnemyPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(_left + 1, _right - 1), _top, 0);
    }

    private void GenerateMovement()
    {
        if (_movementEnabled)
        {
            if (CustomMovementIsSet())
            {
                transform.Translate(_customDirection * _speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(_movementGenerator.GetMovementVector() * _speed * Time.deltaTime);
            }
            
        }
    }

    private bool CustomMovementIsSet()
    {
        return _customDirection.x != 0 || _customDirection.y != 0;
    }

    private void CheckEnemyPosition()
    {
        if (transform.position.y < _bottom)
        {
            ResetPosition();
        }
    }

    public void ChangeMovementTypeTo(IMovement movementType)
    {
        _movementGenerator = movementType;
    }

    public void EnableMovement(bool enable)
    {
        _movementEnabled = enable;
    }

    public void SetCustomDirection(Vector2 newDirection)
    {
        _customDirection = newDirection.normalized;
    }

    public void RemoveCustomDirection()
    {
        _customDirection = new Vector2(0, 0);
    }
}
