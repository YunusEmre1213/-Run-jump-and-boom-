using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Oyun Durumu")]
    public bool isGameOver = false;

    [Header("Skor ve Ekonomi")]
    public int score { get; private set; } = 0;
    public int gold { get; private set; } = 0;
    public int currentAmmo { get; private set; } = 0;

    [Tooltip("Oyun baþýnda oyuncuya verilecek baþlangýç mermisi")]
    public int startingAmmo = 5;

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentAmmo = startingAmmo;
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

    public void AddAmmo(int amount)
    {
        if (isGameOver) return;
        currentAmmo += amount;
    }

    
    public bool TryUseAmmo(int amount = 1)
    {
        if (currentAmmo < amount) return false;

        currentAmmo -= amount;
        return true;
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
        currentAmmo = startingAmmo;
       
    }
}