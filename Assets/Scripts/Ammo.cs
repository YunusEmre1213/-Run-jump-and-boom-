using UnityEngine;

public class Ammo : MonoBehaviour
{
    [Tooltip("Bu pickup toplandýðýnda kazanýlacak mermi miktarý")]
    public int ammoValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddAmmo(ammoValue);
        }

       
        gameObject.SetActive(false);
    }
}