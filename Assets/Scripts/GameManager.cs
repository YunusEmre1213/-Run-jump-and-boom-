using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Oyun Durumu")]
    public bool isGameOver = false;

    [Header("Skor ve Ekonomi")]
    public int score { get; private set; } = 0;
    public int gold { get; private set; } = 0;

    void Awake()
    {
      
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
    }

    public void AddGold(int amount)
    {
        if (isGameOver) return;
        gold += amount;
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return; 

        isGameOver = true;
        Debug.Log($"OYUN BÝTTÝ - Skor: {score}, Altýn: {gold}");

 
    }

    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        gold = 0;
       
    }
}