using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button retryButton;
    [SerializeField] private GameObject gameOverPanel;
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
}
