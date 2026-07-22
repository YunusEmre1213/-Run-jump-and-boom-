using UnityEngine;
using TMPro;

public class DebugHUD : MonoBehaviour
{
    [Header("Referanslar")]
    [Tooltip("Hýzý okuyacaðýmýz Player objesi")]
    public PlayerController player;

    [Header("UI Elemanlarý")]
    [Tooltip("Hýzý gösterecek TextMeshPro metni")]
    public TextMeshProUGUI speedText;

    [Tooltip("Geçen süreyi gösterecek TextMeshPro metni")]
    public TextMeshProUGUI timeText;

    [Tooltip("Skoru gösterecek TextMeshPro metni")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Altýný gösterecek TextMeshPro metni")]
    public TextMeshProUGUI goldText;

    [Tooltip("Mermiyi gösterecek TextMeshPro metni")]
    public TextMeshProUGUI ammoText;

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (speedText != null && player != null)
        {
            speedText.text = $"Hýz: {player.forwardSpeed:F1}";
        }

        if (timeText != null)
        {
            timeText.text = $"Süre: {elapsedTime:F1}s";
        }

        if (scoreText != null && GameManager.Instance != null)
        {
            scoreText.text = $"Skor: {GameManager.Instance.score}";
        }

        if (goldText != null && GameManager.Instance != null)
        {
            goldText.text = $"Altýn: {GameManager.Instance.gold}";
        }

        if (ammoText != null && GameManager.Instance != null)
        {
            ammoText.text = $"Mermi: {GameManager.Instance.currentAmmo}";
        }
    }
}