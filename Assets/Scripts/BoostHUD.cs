using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostHUD : MonoBehaviour
{
    [SerializeField] Sprite _boostOnSprite = null;
    [SerializeField] Sprite _boostCooldownSprite = null;
    [SerializeField] Sprite _boostReadySprite = null;

    private Image _image = null;
    private Text _timeText = null;

    // Start is called before the first frame update
    void Start()
    {
        _timeText = transform.Find("BoostTime").GetComponent<Text>();
        UpdateBoostTime(0.0f);
        _image = GetComponent<Image>();
        SetBoostReady();
    }

    public void UpdateBoostTime(float time)
    {
        _timeText.text = time.ToString("F4");
    }

    public void SetBoostCooldown()
    {
        _image.sprite = _boostCooldownSprite;
    }

    public void SetBoostOn()
    {
        _image.sprite = _boostOnSprite;
    }

    public void SetBoostReady()
    {
        _image.sprite = _boostReadySprite;
    }


}
