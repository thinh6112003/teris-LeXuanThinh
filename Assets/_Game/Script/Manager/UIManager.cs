using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button retryButton;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    private void Awake()
    {
        retryButton.onClick.AddListener(turnOfGameOverPanel);
    }
    public void turnOnGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    public void turnOfGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.Awake();
    }
    public void UpdateScore()
    {
        scoreText.text = GameManager.Instance.score.ToString();
        if (GameManager.Instance.score > GameManager.Instance.highScore) GameManager.Instance.highScore = GameManager.Instance.score;
        highScoreText.text = GameManager.Instance.highScore.ToString();
    }
}
