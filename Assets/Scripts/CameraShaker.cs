using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float _shakeStrength = 0.1f;
    [SerializeField] float _shakeTime = 0.5f;
    private float _currentShakeTime = 0f;
    private Camera _camera = null;
    private bool _shakeActive = false;
    Vector3 _originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _originalPosition = _camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeActive)
        {
            _currentShakeTime += Time.deltaTime;
            var xRand = Random.Range(-_shakeStrength, _shakeStrength);
            var yRand = Random.Range(-_shakeStrength, _shakeStrength);
            Vector3 newPos = _originalPosition;
            newPos.x += xRand;
            newPos.y += yRand;
            _camera.transform.position = newPos;
            if (_currentShakeTime >= _shakeTime)
            {
                _shakeActive = false;
            }
        }
        else
        {
            _camera.transform.position = _originalPosition;
        }
    }


    public void ActivateShake()
    {
        _currentShakeTime = 0;
        _shakeActive = true;        
    }

}
