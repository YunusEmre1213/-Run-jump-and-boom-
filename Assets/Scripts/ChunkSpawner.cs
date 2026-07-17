using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [Header("Referanslar")]
    [Tooltip("Takip edilecek oyuncu (mesafe hesaplamasý için)")]
    public Transform player;

    [Tooltip("Kullanýlacak chunk havuzlarý (her biri farklý bir chunk tipini temsil eder)")]
    public List<ObjectPool> chunkPools;

    [Header("Chunk Ayarlarý")]
    [Tooltip("Her bir chunk'ýn Z ekseni boyunca uzunluðu (tüm chunk'lar ayný uzunlukta olmalý)")]
    public float chunkLength = 20f;

    [Tooltip("Baþlangýçta ve her an aktif tutulacak chunk sayýsý")]
    public int chunksAhead = 4;

    [Tooltip("Bir chunk oyuncunun bu kadar gerisinde kalýnca havuza iade edilir")]
    public float despawnDistanceBehind = 25f;

    
    private List<(GameObject obj, ObjectPool sourcePool)> activeChunks = new();

    private float nextSpawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
       
        if (player.position.z > nextSpawnZ - (chunksAhead * chunkLength))
        {
            SpawnNextChunk();
        }

        DespawnOldChunks();
    }

    private void SpawnNextChunk()
    {
        
        ObjectPool chosenPool = chunkPools[Random.Range(0, chunkPools.Count)];

        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
        GameObject chunk = chosenPool.GetFromPool(spawnPosition, Quaternion.identity);

        activeChunks.Add((chunk, chosenPool));
        nextSpawnZ += chunkLength;
    }

    private void DespawnOldChunks()
    {
        
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            var (obj, sourcePool) = activeChunks[i];

            if (player.position.z - obj.transform.position.z > despawnDistanceBehind + chunkLength)
            {
                sourcePool.ReturnToPool(obj);
                activeChunks.RemoveAt(i);
            }
        }
    }
}