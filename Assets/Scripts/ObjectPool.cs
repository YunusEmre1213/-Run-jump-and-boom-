using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    [Tooltip("Bu havuzun yöneteceði prefab")]
    public GameObject prefab;

    [Tooltip("Havuzda baþlangýçta hazýr bekletilecek obje sayýsý")]
    public int initialSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Prewarm();
    }

    private void Prewarm()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }


    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

   
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}