using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText = null;
    private Image _livesDisplay = null;
    private Text _gameOverMessage = null;
    private GameObject _gameOverScreen = null;
    [SerializeField] Sprite[] _livesSpritesRef = null;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();
        _livesDisplay = transform.Find("LivesDisplay").GetComponent<Image>();
        _gameOverScreen = transform.Find("GameOverScreen").gameObject;
        _gameOverMessage = _gameOverScreen.transform.Find("GameOverText").GetComponent<Text>();
        UpdateScoreText(0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _livesSpritesRef[currentLives];

        if (currentLives <= 0)
        {
            StartCoroutine(ShowGameOverScreen());
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    private IEnumerator ShowGameOverScreen()
    {
        bool textState = true;
        _gameOverScreen.SetActive(true);
        while (true)
        {
            _gameOverMessage.gameObject.SetActive(textState);
            yield return new WaitForSeconds(0.5f);
            textState = !textState;
        }
    }
}
