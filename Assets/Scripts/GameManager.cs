using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Oyun Durumu")]
    public bool isGameOver = false;

    void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return; 

        isGameOver = true;
        Debug.Log("OYUN BÝTTÝ");

       
    }

    public void RestartGame()
    {
        isGameOver = false;
    }
}