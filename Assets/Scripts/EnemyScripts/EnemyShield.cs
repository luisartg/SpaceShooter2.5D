using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{

    bool _shieldActive = false;

    private void Awake()
    {
    }

    public void SetShield(bool isOn)
    {
        gameObject.SetActive(isOn);
        _shieldActive = isOn;
    }

    public bool IsShieldActive()
    {
        return _shieldActive;
    }
}
