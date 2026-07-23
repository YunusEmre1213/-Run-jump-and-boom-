using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    [Tooltip("Patlama efekti prefabýnýn havuzu (PooledEffect scripti içeren bir ObjectPool)")]
    public ObjectPool explosionPool;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnExplosion(Vector3 position)
    {
        if (explosionPool == null) return;

        GameObject fx = explosionPool.GetFromPool(position, Quaternion.identity);

        PooledEffect effect = fx.GetComponent<PooledEffect>();
        if (effect != null)
        {
            effect.sourcePool = explosionPool;
        }
    }
}