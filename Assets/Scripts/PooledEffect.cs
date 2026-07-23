using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PooledEffect : MonoBehaviour
{
    [HideInInspector]
    public ObjectPool sourcePool;

    private ParticleSystem particle;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        particle.Play();
        StartCoroutine(ReturnAfterDuration());
    }

    private IEnumerator ReturnAfterDuration()
    {
        yield return new WaitForSeconds(particle.main.duration);

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