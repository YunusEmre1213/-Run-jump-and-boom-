using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject mainMenuPanel;
    public GameObject gameHUDPanel;
    public GameObject gameOverPanel;

    [Header("Oyun Sonu Ekraný Ýçeriði")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalGoldText;

    private bool gameOverShown;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        
        if (GameManager.Instance != null && GameManager.Instance.isGameOver && !gameOverShown)
        {
            ShowGameOver();
        }
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gameHUDPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameOverShown = false;
    }

    public void OnStartButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        gameHUDPanel.SetActive(true);
        gameOverPanel.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    private void ShowGameOver()
    {
        gameOverShown = true;

        mainMenuPanel.SetActive(false);
        gameHUDPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        if (GameManager.Instance != null)
        {
            if (finalScoreText != null)
                finalScoreText.text = $"Skor: {GameManager.Instance.score}";

            if (finalGoldText != null)
                finalGoldText.text = $"Altýn: {GameManager.Instance.gold}";
        }
    }

   
    public void OnRestartButtonPressed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }

        gameHUDPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gameOverShown = false;
    }
}