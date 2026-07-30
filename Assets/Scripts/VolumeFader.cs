using System.Collections;
using UnityEngine;
using UnityEngine.Rendering; // 必须引入这个命名空间才能调用 Volume

public class VolumeFader : MonoBehaviour
{
    [Header("Volume 设置")]
    [Tooltip("拖入你想要控制的 Global Volume")]
    public Volume targetVolume;

    [Tooltip("过渡所需的总时间（秒）")]
    public float fadeDuration = 2.0f;

    [Header("目标权重设置")]
    [Tooltip("Fade In（淡入）时的目标数值")]
    [Range(0f, 1f)]
    public float fadeInTarget = 1.0f;

    [Tooltip("Fade Out（淡出）时的目标数值")]
    [Range(0f, 1f)]
    public float fadeOutTarget = 0.0f;

    // 用于防止多个渐变协程同时运行
    private Coroutine currentFadeCoroutine;

    void Start()
    {
        // 游戏开始时，默认将权重设置为你定义的淡出初始值
        if (targetVolume != null)
        {
            targetVolume.weight = fadeOutTarget;
        }
    }

    /// <summary>
    /// 触发 Volume 淡入 (当前权重 -> fadeInTarget)
    /// </summary>
    public void FadeInVolume()
    {
        if (targetVolume == null) return;

        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeWeight(targetVolume.weight, fadeInTarget, fadeDuration));
    }

    /// <summary>
    /// 触发 Volume 淡出 (当前权重 -> fadeOutTarget)
    /// </summary>
    public void FadeOutVolume()
    {
        if (targetVolume == null) return;

        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeWeight(targetVolume.weight, fadeOutTarget, fadeDuration));
    }

    /// <summary>
    /// 额外功能：如果你想通过其他脚本直接指定一个特定数值进行渐变，可以调用这个方法
    /// </summary>
    public void FadeToSpecificValue(float specificTarget)
    {
        if (targetVolume == null) return;

        // 确保输入的数值限制在 0 到 1 之间
        specificTarget = Mathf.Clamp01(specificTarget);

        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeWeight(targetVolume.weight, specificTarget, fadeDuration));
    }

    // 核心渐变逻辑（这里不需要修改，它会自动接收上面的目标值）
    private IEnumerator FadeWeight(float startWeight, float endWeight, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            // Mathf.Lerp 用于在两个数值之间进行平滑插值
            targetVolume.weight = Mathf.Lerp(startWeight, endWeight, timer / duration);
            yield return null;
        }

        // 确保最终权重精确到位
        targetVolume.weight = endWeight;
    }
}