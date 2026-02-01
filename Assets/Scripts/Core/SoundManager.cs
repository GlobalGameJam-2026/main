using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip, float fadeDuration = 1.0f)
    {
        // 1. 이미 같은 음악이면 재생 안 함 (이어짐)
        if (audioSource.clip == clip) return;

        // 2. 음악이 아예 꺼져있었다면 바로 재생
        if (audioSource.clip == null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            return;
        }

        // 3. 다른 음악이라면 자연스럽게 페이드 교체 시작
        StartCoroutine(FadeSwitchBGM(clip, fadeDuration));
    }

    // 음악을 서서히 줄이고 -> 교체하고 -> 서서히 키우는 코루틴
    IEnumerator FadeSwitchBGM(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;
        float halfDuration = duration * 0.5f;

        // 1. 볼륨 줄이기 (Fade Out)
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / halfDuration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();

        // 2. 음악 교체 및 재생
        audioSource.clip = newClip;
        audioSource.Play();

        // 3. 볼륨 키우기 (Fade In)
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / halfDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}