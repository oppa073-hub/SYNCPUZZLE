using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [Range(0f, 1f)] public float bgmVolume = 0.6f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f; 
    private Coroutine fadeCo;

    [SerializeField] private AudioClip selectSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip disappearSfx;
    [SerializeField] private AudioClip bumpSfx;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null || bgmSource == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
    public void PlayJump()
    {
        sfxSource.PlayOneShot(jumpSfx, sfxVolume);
    }
    public void PlaySelect()
    {
        sfxSource.PlayOneShot(selectSfx, sfxVolume);
    }
    public void FadeBGM(AudioClip nextClip, float fadeOut = 0.3f, float fadeIn = 0.3f)
    {
        if (fadeCo != null) StopCoroutine(fadeCo);
        fadeCo = StartCoroutine(FadeRoutine(nextClip, fadeOut, fadeIn));
    }
    private IEnumerator FadeRoutine(AudioClip nextClip, float outTime, float inTime)
    {
        if (bgmSource == null) yield break;

        float start = bgmSource.volume;
        for (float t = 0; t < outTime; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(start, 0f, t / outTime);
            yield return null;
        }
        bgmSource.volume = 0f;
        if (nextClip != null)
        {
            bgmSource.clip = nextClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        for (float t = 0; t < inTime; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0f, bgmVolume, t / inTime);
            yield return null;
        }
        bgmSource.volume = bgmVolume;
        fadeCo = null;

    }
}
