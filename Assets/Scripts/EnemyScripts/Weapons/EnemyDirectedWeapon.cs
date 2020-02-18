using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EnemyScripts.Weapons;

public class EnemyDirectedWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] int _shotMaxTime = 10;
    [SerializeField] int _shotMinTime = 0;
    [SerializeField] LaserWithDirection _laser = null;
    [SerializeField] AudioClip _laserSound = null;
    [SerializeField] Vector2 _laserDirection = new Vector2(0, -1);

    AudioSource _audioSource = null;
    private EnemyCore _enemyCore = null;

    private void Awake()
    {
        _enemyCore = GetComponent<EnemyCore>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootProcess());
    }

    private IEnumerator ShootProcess()
    {
        while (_enemyCore.IsAlive())
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_shotMinTime, _shotMaxTime + 1));
            Shoot(_laserDirection);
        }
    }

    public void Shoot(Vector2 direction)
    {
        if (_enemyCore.IsAlive())
        {
            LaserWithDirection laser = Instantiate(_laser, transform.position, Quaternion.identity);
            laser.SetDirection(direction);
            AudioSource.PlayClipAtPoint(_laserSound, Camera.main.transform.position);
        }
    }
}
