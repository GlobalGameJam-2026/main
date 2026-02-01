using UnityEngine;
using UnityEngine.SceneManagement;

// 1. �� ��ũ��Ʈ�� �߰��ϸ� BoxCollider2D�� �ڵ����� ���� �ٽ��ϴ�.
// (�̹� �ִٸ� �״�� �ΰ�, ���ٸ� ���� �����մϴ�)
[RequireComponent(typeof(BoxCollider2D))]
public class Hazard : MonoBehaviour
{
    // 2. ��ũ��Ʈ�� ������Ʈ�� ó�� �߰��� ��(�����Ϳ�����) �ڵ����� ����Ǵ� �Լ��Դϴ�.
    private void Reset()
    {
        // �ڵ����� ���� �ڽ� �ݶ��̴��� �����ɴϴ�.
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        // 'Is Trigger' �ɼ��� �ڵ����� ���ݴϴ�. (�������� �� �ʿ� ����)
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}