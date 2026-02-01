using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Hazard : MonoBehaviour
{
    [Tooltip("여기에 효과음 파일을 드래그해서 넣으세요")]
    public AudioClip hitSound;
    
    [Tooltip("캐릭터가 완전히 사라지는 데 걸리는 시간 (초 단위)")]
    public float fadeDuration = 0.5f; // 소리 길이와 별개로 페이드 아웃 시간 설정

    private AudioSource audioSource;
    private bool isTriggered = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Reset()
    {
        // 컴포넌트 자동 설정 (이전과 동일)
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null) boxCollider.isTrigger = true;
        AudioSource aSource = GetComponent<AudioSource>();
        if (aSource != null) aSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            // 플레이어의 관련 컴포넌트들을 가져옵니다.
            Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
            SpriteRenderer playerSprite = other.GetComponentInChildren<SpriteRenderer>();

            // 코루틴 시작 시 플레이어 정보를 전달합니다.
            StartCoroutine(ResetSequence(playerRB, playerSprite));
        }
    }

    // 플레이어 정보를 받아서 처리하는 코루틴
    IEnumerator ResetSequence(Rigidbody2D playerRB, SpriteRenderer playerSprite)
    {
        isTriggered = true;

        // --- 1단계: 플레이어 정지 및 효과음 재생 ---
        
        // 물리 시뮬레이션을 꺼서 그 자리에 얼어붙게 만듭니다.
        if (playerRB != null)
        {
            playerRB.linearVelocity = Vector2.zero; // 현재 속도 제거
            playerRB.simulated = false;      // 물리 연산에서 제외 (중력, 충돌 무시)
        }

        // 효과음 재생
        float soundDuration = 0f;
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
            soundDuration = hitSound.length;
        }

        // --- 2단계: 소멸 (페이드 아웃) 연출 ---

        if (playerSprite != null)
        {
            float elapsedTime = 0f;
            Color startColor = playerSprite.color;

            // fadeDuration 동안 반복하면서 투명도를 조절합니다.
            while (elapsedTime < fadeDuration)
            {
                // 시간 경과에 따라 1(불투명)에서 0(투명)으로 값을 변경
                float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                playerSprite.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
                
                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            // 루프가 끝난 후 완전히 투명하게 설정
            playerSprite.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }

        // --- 3단계: 남은 소리 시간 대기 ---

        // 페이드 아웃이 끝났는데 소리가 아직 남았다면 마저 기다립니다.
        if (soundDuration > fadeDuration)
        {
             yield return new WaitForSeconds(soundDuration - fadeDuration);
        }
        // 만약 소리가 페이드 아웃보다 짧거나 없다면, 최소 페이드 아웃 시간만큼은 기다린 셈이 됩니다.

        // --- 4단계: 씬 리셋 ---
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}