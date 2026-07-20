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
    }
}