using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
    // 데미지 텍스트가 화면에 표시될 시간
    public float displayDuration = 1f;

    void Start()
    {
        // 지정된 시간 후에 DestroyCanvas 코루틴 시작
        StartCoroutine(DestroyCanvasAfterDelay(displayDuration));
    }

    IEnumerator DestroyCanvasAfterDelay(float delay)
    {
        // 지정된 시간(1초)만큼 대기
        yield return new WaitForSeconds(delay);

        // 오브젝트 파괴
        Destroy(gameObject);
    }
}