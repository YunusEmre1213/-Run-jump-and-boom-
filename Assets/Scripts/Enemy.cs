using System.Collections;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [Header("Skor")]
    [Tooltip("Bu düþman vurulduðunda kazanýlacak skor")]
    public int scoreValue = 20;

    [Header("Zamanlama Penceresi")]
    [Tooltip("Düþmanýn vurulabilir kalacaðý süre (saniye) — SADECE oyuncu yeterince yaklaþtýktan sonra baþlar")]
    public float timeToLive = 1f;

    [Tooltip("Oyuncu düþmana bu mesafeden (Z ekseninde) daha yakýna gelmeden geri sayým baþlamaz")]
    public float activationDistance = 12f;

    [Tooltip("Geri sayýmý gösteren halka objesi (opsiyonel, boþ býrakýlabilir)")]
    public Transform ringIndicator;

    
    [HideInInspector]
    public ObjectPool sourcePool;

    private Coroutine countdownCoroutine;
    private Vector3 ringInitialScale;
    private Transform playerTransform;
    private bool countdownStarted;

    void Awake()
    {
        if (ringIndicator != null)
        {
            ringInitialScale = ringIndicator.localScale;
        }

       
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    
    void OnEnable()
    {
        if (ringIndicator != null)
        {
            ringIndicator.localScale = ringInitialScale;
        }

        countdownStarted = false;
    }

    void OnDisable()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    void Update()
    {
        if (countdownStarted || playerTransform == null) return;

        float distance = transform.position.z - playerTransform.position.z;

        if (distance <= activationDistance)
        {
            countdownStarted = true;
            countdownCoroutine = StartCoroutine(CountdownRoutine());
        }
    }

    private IEnumerator CountdownRoutine()
    {
        float elapsed = 0f;

        while (elapsed < timeToLive)
        {
            elapsed += Time.deltaTime;

            if (ringIndicator != null)
            {
                float remainingRatio = 1f - (elapsed / timeToLive);
                ringIndicator.localScale = ringInitialScale * remainingRatio;
            }

            yield return null;
        }

        
        OnMissed();
    }

   
    public void OnHit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemyHit(scoreValue);
        }

       

        ReturnToPool();
    }

   
    private void OnMissed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(1);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (sourcePool != null)
        {
            sourcePool.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}