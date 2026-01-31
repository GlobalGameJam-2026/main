using ThawTheMask;
using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MonoBehaviour
{
    [Header("UI 연결")]
    public Image iconImage;       // 가면 아이콘 (자식)

    [Header("하얀 가면 모드일 때 (White Mask)")]
    public Sprite whiteMaskIcon;  // 하얀 가면 아이콘 그림

    [Header("검은 가면 모드일 때 (Black Mask)")]
    public Sprite blackMaskIcon;  // 검은 가면 아이콘 그림

    void Start()
    {
        UpdateMaskUI();
    }

    void Update()
    {
        // 최적화를 원하면 PlayerController에서 키를 눌렀을 때만 호출하도록 변경 가능
        UpdateMaskUI();
    }

    public void UpdateMaskUI()
    {
        if (MaskManager.Instance == null)
        {
            Debug.LogWarning("MaskManager not found in scene! Please add MaskManager to the scene.");
            return;
        }
        

        if (MaskManager.Instance.CurrentMask == MaskType.White)
        {
            // 하얀 가면 상태: 아이콘과 배경을 해당 모드 것으로 교체
            iconImage.sprite = whiteMaskIcon;
        }
        else
        {
            // 검은 가면 상태: 아이콘과 배경을 해당 모드 것으로 교체
            iconImage.sprite = blackMaskIcon;
        }
    }
}