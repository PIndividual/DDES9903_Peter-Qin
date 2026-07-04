using System.Collections;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

public class TextReveal : MonoBehaviour
{
    [Header("组件引用")]
    [Tooltip("拖入场景中的 TextMeshPro 文字对象")]
    public TMP_Text textMesh;

    [Header("时间设置")]
    [Tooltip("文字全部显示完成所需的总时间（秒）")]
    public float duration;

    // 触发打字机效果的方法
    public void StartTypewriterEffect()
    {
        if (textMesh == null)
        {
            Debug.LogWarning("缺少 TextMeshPro 组件，请在面板中指定！");
            return;
        }

        // 停止可能正在运行的同一个协程，防止多次重复触发导致乱码
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        // 强制更新网格信息，确保能获取到最准确的总字符数
        textMesh.ForceMeshUpdate();
        int totalCharacters = textMesh.textInfo.characterCount;

        // 初始状态下隐藏所有字符
        textMesh.maxVisibleCharacters = 0;

        float timer = 0f;

        // 在设定的 duration 时间内逐步增加显示的字符数
        while (timer < duration)
        {
            timer += Time.deltaTime;

            // 计算当前时间占总时间的百分比
            float percent = timer / duration;

            // 根据百分比向下取整，算出当前应该显示几个字
            textMesh.maxVisibleCharacters = Mathf.FloorToInt(totalCharacters * percent);

            yield return null; // 等待下一帧
        }

        // 循环结束后，确保所有文字 100% 显示完毕
        textMesh.maxVisibleCharacters = totalCharacters;
    }
}