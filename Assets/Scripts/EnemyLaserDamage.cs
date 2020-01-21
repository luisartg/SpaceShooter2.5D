using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserDamage : MonoBehaviour
{
    [SerializeField] AudioClip _laserShock = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().Damage();
            AudioSource.PlayClipAtPoint(_laserShock, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
