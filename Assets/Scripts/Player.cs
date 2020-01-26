using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int _lives = 3;

    [SerializeField] float _speed = 1f;
    [SerializeField] float _boostSpeed = 3f;
    [SerializeField] float _thrusterSpeed = 3f;
    [SerializeField] float _maxY = 3f;
    [SerializeField] float _minY = -3f;
    [SerializeField] float _maxX = 11.5f;
    [SerializeField] float _minX = -11.5f;

    [SerializeField] GameObject _laserPrefab = null;
    [SerializeField] GameObject _tripleLaserPrefab = null;
    [SerializeField] float _firerate = 0.15f;
    [SerializeField] int _ammo = 15;

    [SerializeField] SpawnManager _spawnManager = null;
    [SerializeField] bool _tripleShotEnabled = false;
    [SerializeField] int _powerUpPeriodSeconds = 6;

    private float _timeLeftToFire = -1f;

    private GameObject _shield = null;
    private bool _shieldActive = false;
    private int _shieldCurrentStrength = 0;
    [SerializeField] private Color[] _shieldStrengthColors = new Color[] { Color.red, Color.yellow, Color.white };

    private bool _boostActive = false;

    [SerializeField] int _score = 0;
    private UIManager _uiManagerRef = null;

    [SerializeField] GameObject[] _playerHits = null;

    [SerializeField] AudioClip _laserSound = null;
    [SerializeField] AudioClip _outOfAmmoSound = null;
    [SerializeField] AudioClip _explosionSound = null;
    AudioSource _audioSource = null;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _shield = transform.Find("Shield").gameObject;
        ActivateShield(false);
        _uiManagerRef = GameObject.Find("Canvas").GetComponent<UIManager>();

        _playerHits = new GameObject[2];
        _playerHits[0] = transform.Find("PlayerHitRight").gameObject;
        _playerHits[1] = transform.Find("PlayerHitLeft").gameObject;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserSound;

        if (!_spawnManager) Debug.Log("The Spawn Manager is null");
        if (!_uiManagerRef) Debug.Log("The UI Manager is null");

        _uiManagerRef.UpdateAmmoText(_ammo);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lives > 0)
        {
            CalculateMovement();
            Fire();
        }
    }

    private void CalculateMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(GetMovementValue(horizontalAxis),
                                        GetMovementValue(verticalAxis),
                                        0));
        LimitPositionY();
        LimitPositionX();
    }

    private float GetMovementValue(float axisValue)
    {
        float currentSpeed = _speed;
        if (_boostActive)
        {
            currentSpeed += _boostSpeed; 
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed += _thrusterSpeed;
        }
        return axisValue * currentSpeed * Time.deltaTime;
    }

    private void LimitPositionY()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _minY, _maxY), 0);
    }

    private void LimitPositionX()
    {
        if (transform.position.x > _maxX)
        {
            transform.position = new Vector3(_minX, transform.position.y, 0);
        }

        if (transform.position.x < _minX)
        {
            transform.position = new Vector3(_maxX, transform.position.y, 0);
        }
    }

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timeLeftToFire)
        {
            _timeLeftToFire = Time.time + _firerate;
            if (_ammo > 0)
            {
                _ammo--;
                _uiManagerRef.UpdateAmmoText(_ammo);
                if (_tripleShotEnabled)
                {
                    Instantiate(_tripleLaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                }
                _audioSource.clip = _laserSound;
                _audioSource.Play();
            }
            else
            {
                _audioSource.clip = _outOfAmmoSound;
                _audioSource.Play();
                return;
            }
        }
    }

    public void Damage()
    {

        if (_shieldActive)
        {
            ActivateShield(false);
            return;
        }
        _lives--;
        ShowPlayerHits();
        _uiManagerRef.UpdateLives(_lives);
        if (_lives <= 0)
        {
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        _spawnManager.StopSpawning();
        _audioSource.clip = _explosionSound;
        _audioSource.Play();
        GetComponent<Collider2D>().enabled = false;
        HideGraphics();
        Destroy(gameObject, 3.2f);
    }

    private void HideGraphics()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ShowPlayerHits()
    {
        if (_lives > 0 && _lives <= 2)
        {
            _playerHits[_lives - 1].SetActive(true);
        }
    }

    public void ActivateTripleShot()
    {
        Debug.Log("Triple Shot Activated");
        _tripleShotEnabled = true;
        StartCoroutine(DeactivateTripleShot());
    }

    private IEnumerator DeactivateTripleShot()
    {
        yield return new WaitForSeconds(_powerUpPeriodSeconds);
        _tripleShotEnabled = false;
    }

    public void ActivateSpeedBoost()
    {
        Debug.Log("Speed boost active");
        _boostActive = true;
        StartCoroutine(DeactivateSpeedBoost());
    }

    private IEnumerator DeactivateSpeedBoost()
    {
        yield return new WaitForSeconds(_powerUpPeriodSeconds);
        _boostActive = false;
    }

    public void ActivateShield(bool isActive)
    {
        if (isActive)
        {
            _shieldActive = true;
            _shieldCurrentStrength = _shieldStrengthColors.Length;
            _shield.GetComponent<SpriteRenderer>().color = _shieldStrengthColors[_shieldCurrentStrength - 1];
            _shield.SetActive(true);
        }
        else
        {
            _shieldCurrentStrength--;
            if (_shieldCurrentStrength <= 0)
            {
                _shieldActive = false;
                _shield.SetActive(false);
            }
            else
            {
                _shield.GetComponent<SpriteRenderer>().color = _shieldStrengthColors[_shieldCurrentStrength - 1];
            }
        }
        
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManagerRef.UpdateScoreText(_score);
    }

}
