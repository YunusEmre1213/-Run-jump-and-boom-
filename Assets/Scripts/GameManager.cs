using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Oyun Durumu")]
    public bool isGameOver = false;
    public bool isGameStarted { get; private set; } = false;

    [Header("Skor ve Ekonomi")]
    public int score { get; private set; } = 0;
    public int gold { get; private set; } = 0;
    public int currentAmmo { get; private set; } = 0;

    [Tooltip("Oyun baþýnda oyuncuya verilecek baþlangýç mermisi")]
    public int startingAmmo = 5;

    [Header("Can Sistemi")]
    public int currentHealth { get; private set; } = 3;

    [Tooltip("Oyun baþýndaki can sayýsý")]
    public int startingHealth = 3;

    [Header("Combo Sistemi")]
    public int comboCount { get; private set; } = 0;

    [Tooltip("Her combo artýþýnýn skor çarpanýna eklediði miktar (örn. 0.5 -> 3 combo'da x2.5)")]
    public float comboMultiplierStep = 0.5f;

    [Tooltip("Skor çarpanýnýn çýkabileceði en yüksek deðer")]
    public float maxComboMultiplier = 5f;

    public float currentComboMultiplier => 1f + (comboCount * comboMultiplierStep) > maxComboMultiplier
        ? maxComboMultiplier
        : 1f + (comboCount * comboMultiplierStep);

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentAmmo = startingAmmo;
        currentHealth = startingHealth;
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
    }

    public void StartGame()
    {
        isGameStarted = true;
    }

   
    public void RegisterEnemyHit(int baseScoreValue)
    {
        if (isGameOver) return;

        comboCount++;
        int finalScore = Mathf.RoundToInt(baseScoreValue * currentComboMultiplier);
        score += finalScore;
    }

   
    public void ResetCombo()
    {
        comboCount = 0;
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

    
    public void TakeDamage(int amount = 1)
    {
        if (isGameOver) return;

        currentHealth -= amount;
        ResetCombo();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            TriggerGameOver();
        }
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
        isGameStarted = true;
        score = 0;
        gold = 0;
        currentAmmo = startingAmmo;
        currentHealth = startingHealth;
        comboCount = 0;
       
    }
}