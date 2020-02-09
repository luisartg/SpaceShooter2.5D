using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandardWeapon : MonoBehaviour
{
    [SerializeField] int _shotMaxTime = 10;
    [SerializeField] int _shotMinTime = 0;
    [SerializeField] Laser _laser = null;
    [SerializeField] AudioClip _laserSound = null;

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
        StartCoroutine(Shoot());
    }
    
    private IEnumerator Shoot()
    {
        while (_enemyCore.IsAlive())
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_shotMinTime, _shotMaxTime + 1));
            if (_enemyCore.IsAlive())
            {
                Laser laser = Instantiate(_laser, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(_laserSound, Camera.main.transform.position);
            }
        }
    }
}
