using UnityEngine;


public class Enemy : MonoBehaviour
{
    [Tooltip("Bu düþman vurulduðunda kazanýlacak skor")]
    public int scoreValue = 20;

 
    [HideInInspector]
    public ObjectPool sourcePool;

    
    public void OnHit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        

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