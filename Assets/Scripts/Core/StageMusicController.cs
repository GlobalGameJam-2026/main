using UnityEngine;

public class StageMusicController : MonoBehaviour
{
    [Header("이 스테이지 배경음악 설정")]
    [Tooltip("여기에 해당 스테이지에서 재생할 BGM 오디오 클립을 넣으세요.")]
    public AudioClip stageBGM;

    void Start()
    {
        // SoundManager가 존재하는지 확인 (에러 방지)
        if (SoundManager.instance != null)
        {
            // SoundManager에게 음악 재생 요청
            // (SoundManager 내부 로직에 의해, 이미 같은 음악이 나오고 있다면 유지됩니다)
            SoundManager.instance.PlayBGM(stageBGM);
        }
        else
        {
            Debug.LogWarning("씬에 SoundManager가 없습니다! MainMenu 씬부터 시작하거나 SoundManager 프리팹을 배치하세요.");
        }
    }
}