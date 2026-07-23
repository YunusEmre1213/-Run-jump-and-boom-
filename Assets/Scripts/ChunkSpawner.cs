using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ChunkOption
    {
        public ObjectPool pool;

        [Tooltip("1 = kolay, 3 = zor. Zaman ilerledikçe yüksek zorluktaki chunk'larýn seçilme olasýlýðý artar.")]
        [Range(1, 3)]
        public int difficultyTier = 1;
    }

    [Header("Referanslar")]
    [Tooltip("Takip edilecek oyuncu (mesafe hesaplamasý için)")]
    public Transform player;

    [Tooltip("Kullanýlacak chunk seçenekleri, her biri kendi zorluk seviyesiyle")]
    public List<ChunkOption> chunkOptions;

    [Header("Chunk Ayarlarý")]
    [Tooltip("Her bir chunk'ýn Z ekseni boyunca uzunluðu (tüm chunk'lar ayný uzunlukta olmalý)")]
    public float chunkLength = 20f;

    [Tooltip("Baþlangýçta ve her an aktif tutulacak chunk sayýsý")]
    public int chunksAhead = 4;

    [Tooltip("Bir chunk oyuncunun bu kadar gerisinde kalýnca havuza iade edilir")]
    public float despawnDistanceBehind = 25f;

    [Header("Zorluk Ayarlarý")]
    [Tooltip("Zorluðun tam seviyeye (en zor chunk'larýn baskýn olduðu noktaya) ulaþmasý ne kadar sürer (saniye)")]
    public float difficultyRampTime = 90f;

    [Header("Düþman Ayarlarý")]
    [Tooltip("Düþman prefabýnýn havuzu")]
    public ObjectPool enemyPool;

    [Range(0f, 1f)]
    [Tooltip("Bir chunk'taki her düþman spawn noktasýnda düþman çýkma olasýlýðý")]
    public float enemySpawnChance = 0.4f;

    private List<(GameObject obj, ObjectPool sourcePool, List<GameObject> enemies)> activeChunks = new();
    private float nextSpawnZ = 0f;
    private float elapsedTime = 0f;

    void Start()
    {
        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        
        if (GameManager.Instance != null && (!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)) return;

        elapsedTime += Time.deltaTime;

        if (player.position.z > nextSpawnZ - (chunksAhead * chunkLength))
        {
            SpawnNextChunk();
        }

        DespawnOldChunks();
    }

    private void SpawnNextChunk()
    {
        ChunkOption chosen = GetWeightedRandomChunk();

        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
        GameObject chunk = chosen.pool.GetFromPool(spawnPosition, Quaternion.identity);

        List<GameObject> spawnedEnemies = SpawnEnemiesForChunk(chunk);

        activeChunks.Add((chunk, chosen.pool, spawnedEnemies));
        nextSpawnZ += chunkLength;
    }

   
    private List<GameObject> SpawnEnemiesForChunk(GameObject chunk)
    {
        List<GameObject> spawned = new List<GameObject>();

        if (enemyPool == null) return spawned;

        EnemySpawnPoint[] spawnPoints = chunk.GetComponentsInChildren<EnemySpawnPoint>();

        foreach (var point in spawnPoints)
        {
            if (Random.value <= enemySpawnChance)
            {
                GameObject enemy = enemyPool.GetFromPool(point.transform.position, point.transform.rotation);

                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.sourcePool = enemyPool;
                }

                spawned.Add(enemy);
            }
        }

        return spawned;
    }

    
    private float GetDifficultyProgress()
    {
        return 1f - Mathf.Exp(-elapsedTime / difficultyRampTime);
    }

    
    private ChunkOption GetWeightedRandomChunk()
    {
        float progress = GetDifficultyProgress();

        List<float> weights = new List<float>();
        float totalWeight = 0f;

        foreach (var option in chunkOptions)
        {
            
            float easyWeight = 4f - option.difficultyTier;   
            float hardWeight = option.difficultyTier;         
            float weight = Mathf.Lerp(easyWeight, hardWeight, progress);
            weight = Mathf.Max(weight, 0.1f); 

            weights.Add(weight);
            totalWeight += weight;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < chunkOptions.Count; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
            {
                return chunkOptions[i];
            }
        }

        
        return chunkOptions[chunkOptions.Count - 1];
    }

    private void DespawnOldChunks()
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            var (obj, sourcePool, enemies) = activeChunks[i];

            if (player.position.z - obj.transform.position.z > despawnDistanceBehind + chunkLength)
            {
                sourcePool.ReturnToPool(obj);

                foreach (var enemy in enemies)
                {
                    if (enemy != null && enemy.activeSelf)
                    {
                        enemyPool.ReturnToPool(enemy);
                    }
                }

                activeChunks.RemoveAt(i);
            }
        }
    }
}