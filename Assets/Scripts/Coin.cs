using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("Bu coin toplandýðýnda kazanýlacak skor miktarý")]
    public int scoreValue = 10;

    [Tooltip("Bu coin toplandýðýnda kazanýlacak altýn miktarý")]
    public int goldValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.AddGold(goldValue);
        }

        gameObject.SetActive(false);
    }
}