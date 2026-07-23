using UnityEngine;
using UnityEngine.InputSystem;


public class ShootingController : MonoBehaviour
{
    [Header("Referanslar")]
    [Tooltip("Ray göndereceðimiz kamera. Boþ býrakýlýrsa Camera.main kullanýlýr.")]
    public Camera mainCamera;

    [Header("Ayarlar")]
    [Tooltip("Raycast'in ne kadar uzaða kadar isabet arayacaðý")]
    public float raycastDistance = 100f;

    [Tooltip("Bir vuruþ için harcanacak mermi miktarý")]
    public int ammoCostPerShot = 1;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        HandleTouchShoot();
    }

    private void HandleTouchShoot()
    {
        
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            TryShootAt(screenPos);
        }

        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            TryShootAt(screenPos);
        }
    }

    private void TryShootAt(Vector2 screenPosition)
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                TryHitEnemy(hit.collider.gameObject);
            }
        }
    }

    private void TryHitEnemy(GameObject enemy)
    {
        
        if (GameManager.Instance == null || !GameManager.Instance.TryUseAmmo(ammoCostPerShot))
        {
            Debug.Log("Mermi yetersiz, ateþ edilemedi.");
            return;
        }

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnHit();
        }
    }
}