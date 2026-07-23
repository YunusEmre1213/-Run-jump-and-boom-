using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Ses Klipleri (Placeholder)")]
    [Tooltip("Düþman vurulduðunda çalacak ses")]
    public AudioClip enemyHitClip;

    [Tooltip("Engele çarpýldýðýnda / can kaybedildiðinde çalacak ses")]
    public AudioClip damageClip;

    [Tooltip("Coin toplandýðýnda çalacak ses")]
    public AudioClip coinClip;

    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEnemyHit()
    {
        PlayClip(enemyHitClip);
    }

    public void PlayDamage()
    {
        PlayClip(damageClip);
    }

    public void PlayCoin()
    {
        PlayClip(coinClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        audioSource.PlayOneShot(clip);
    }
}