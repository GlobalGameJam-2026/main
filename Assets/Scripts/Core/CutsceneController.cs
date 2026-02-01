using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName; // 이동할 씬 이름

    void Start()
    {
        // 영상이 끝나는 시점을 감지하는 이벤트 연결
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // 영상 종료 후 씬 이동
        SceneManager.LoadScene(nextSceneName);
    }
}