using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperWeapon : MonoBehaviour
{
    [SerializeField] GameObject _laser = null;
    [SerializeField] AudioClip _laserSound = null;
    [SerializeField] AudioClip _warningSound = null;

    [SerializeField] private Material _sightMaterial = null;

    private AudioSource _audioSource = null;
    private EnemyCore _enemyCore = null;
    private Player _playerRef = null;
    private LineRenderer _sightLine = null;
    private int _sightState = 0;
    private float _blinkTime = 0.05f;
    private float _blinkCurrentTime = 0;
    private bool _sightOn = true;

    private void Awake()
    {
        _enemyCore = GetComponent<EnemyCore>();
        _audioSource = GetComponent<AudioSource>();
        _playerRef = FindObjectOfType<Player>();
        _sightLine = gameObject.AddComponent<LineRenderer>();
    }

    // Cycle
    // Set a shot line to the player and follow the player
    // After x seconds start the warning, and start blinking shoot line 
    // After y seconds turn off shot line and shoot in the las direction of the line
    // wait z seconds and start again

    // Start is called before the first frame update
    void Start()
    {
        FormatSighLine();
        //StartCoroutine(Shoot());
        StartCoroutine(SightStateCycle());
    }

    private IEnumerator SightStateCycle()
    {
        while (_enemyCore.IsAlive())
        {
            _sightState = 0;
            yield return new WaitForSeconds(2);

            if (!_enemyCore.IsAlive()) break;
            _audioSource.clip = _warningSound;
            _audioSource.Play();
            _sightState = 1;
            yield return new WaitForSeconds(1.2f);

            if (!_enemyCore.IsAlive()) break;
            _sightState = 3;
            Shoot();
            yield return new WaitForSeconds(3);
        }
    }

    private void FormatSighLine()
    {
        _sightLine.startColor = Color.red;
        _sightLine.endColor = Color.clear;
        _sightLine.material = _sightMaterial;
        _sightLine.startWidth = 0.04f;
        _sightLine.endWidth = 0.01f;
    }

    private void Update()
    {
        if (_enemyCore.IsAlive())
        {
            ShowSight();
            ShowWarning();
            TurnOffSight();
        }
        else
        {
            SetSightVisibility(false);
            _audioSource.Stop();
        }
    }

    private void TurnOffSight()
    {
        if (_sightState == 3)
        {
            SetSightVisibility(false);
        }
    }

    private void ShowSight()
    {
        if (_sightState == 0)
        {
            SetSightVisibility(true);
            SetSightToPlayer();
        }
    }

    private void SetSightToPlayer()
    {
        _sightLine.SetPositions(new Vector3[] { _playerRef.transform.position, transform.position });
    }

    private void SetSightVisibility(bool show)
    {
        _sightLine.enabled = show;
    }

    private void ShowWarning()
    {
        if (_sightState == 1)
        {
            _blinkCurrentTime += Time.deltaTime;
            if (_blinkCurrentTime > _blinkTime)
            {
                _blinkCurrentTime = 0;
                _sightOn = !_sightOn;
            }

            SetSightToPlayer();

            if (_sightOn)
            {
                SetSightVisibility(true);
            }
            else
            {
                SetSightVisibility(false);
            }
        }
    }

    private void Shoot()
    {
        if (_enemyCore.IsAlive())
        {
            Vector2 direction = GetDirection();
            LaserWithDirection laser = Instantiate(_laser, transform.position, Quaternion.identity).GetComponent<LaserWithDirection>();
            laser.SetDirection(direction);
            _audioSource.clip = _laserSound;
            _audioSource.Play();
        }
    }

    private Vector2 GetDirection()
    {
        float a = _playerRef.transform.position.y - transform.position.y;
        float b = _playerRef.transform.position.x - transform.position.x;
        return new Vector2(b, a).normalized;
    }

}
