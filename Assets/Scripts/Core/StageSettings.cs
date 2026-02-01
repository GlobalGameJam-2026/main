using System.Collections;
using UnityEngine;

namespace ThawTheMask
{
    public class StageSettings : MonoBehaviour
    {
        [Header("Stage Backgrounds")]
        public Sprite WhiteModeBackground;
        public Sprite BlackModeBackground;

        private void Start()
        {
            // 아주 약간의 딜레이를 주어 매니저 초기화 순서를 보장 (안전장치)
            StartCoroutine(InitBackground());
        }

        private IEnumerator InitBackground()
        {
            // 같은 프레임 내 다른 스크립트의 실행을 기다림
            yield return null;

            if (StageBackgroundManager.Instance != null)
            {
                // 매니저에게 내 스프라이트를 전달하고 즉시 갱신(Refresh) 요청
                StageBackgroundManager.Instance.SetStageSprites(WhiteModeBackground, BlackModeBackground);
                Debug.Log($"[StageSettings] 배경 매니저에 현재 스테이지 스프라이트 적용 완료");
            }
            else
            {
                Debug.LogError("[StageSettings] StageBackgroundManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }
}