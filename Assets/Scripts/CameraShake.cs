using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        Instance = this;
        originalLocalPosition = transform.localPosition;
    }

   
    public void Shake(float duration = 0.15f, float magnitude = 0.2f)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalLocalPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
          
            float damper = 1f - (elapsed / duration);
            float offsetX = Random.Range(-1f, 1f) * magnitude * damper;
            float offsetY = Random.Range(-1f, 1f) * magnitude * damper;

            transform.localPosition = originalLocalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
        shakeCoroutine = null;
    }
}