using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VideoPlayer))]
public class VideoSceneLoader : MonoBehaviour
{
    public string nextSceneName;
    private VideoPlayer videoPlayer;

    void Start()
    {
        // 1. 비디오 시작 전 모든 소리 끄기
        StopAllOtherSounds();

        // 2. 비디오 플레이어 설정
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void StopAllOtherSounds()
    {
        // 씬에 활성화된 모든 오디오 소스를 찾아 배열에 담습니다.
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            // (중요) 만약 비디오 소리가 AudioSource를 통해 나온다면, 
            // 자기 자신(비디오 소리)은 끄면 안 되므로 예외 처리합니다.
            if (source.gameObject == this.gameObject) continue;

            source.Stop();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}