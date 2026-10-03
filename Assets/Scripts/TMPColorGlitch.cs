using UnityEngine;
using TMPro;
using System.Collections;

public class TMPColorGlitch : MonoBehaviour
{
    [Header("文字组件")]

    [Tooltip("需要产生颜色抽动的 TMP 文字")]
    public TMP_Text targetText;


    [Header("触发间隔")]

    [Tooltip("两次颜色抽动之间的最短等待时间")]
    [Min(0f)]
    public float minInterval = 2f;

    [Tooltip("两次颜色抽动之间的最长等待时间")]
    [Min(0f)]
    public float maxInterval = 6f;


    [Header("抽动设置")]

    [Tooltip("一次抽动最少闪几次")]
    [Min(1)]
    public int minGlitchCount = 1;

    [Tooltip("一次抽动最多闪几次")]
    [Min(1)]
    public int maxGlitchCount = 3;

    [Tooltip("每次随机颜色最短保持时间")]
    [Min(0f)]
    public float minColorDuration = 0.03f;

    [Tooltip("每次随机颜色最长保持时间")]
    [Min(0f)]
    public float maxColorDuration = 0.12f;


    [Header("颜色设置")]

    [Tooltip("开启后完全随机生成 RGB 颜色")]
    public bool useRandomRGB = false;

    [Tooltip("关闭 Random RGB 后，从这些颜色中随机选择")]
    public Color[] glitchColors =
    {
        Color.red,
        Color.cyan,
        Color.magenta,
        Color.yellow
    };


    [Header("恢复设置")]

    [Tooltip("每轮抽动结束后恢复原来的颜色")]
    public bool returnToOriginalColor = true;


    [Header("时间设置")]

    [Tooltip("不受 Time.timeScale 影响")]
    public bool useUnscaledTime = true;


    private Color originalColor;

    private Coroutine glitchCoroutine;


    private void Awake()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TMP_Text>();
        }

        if (targetText != null)
        {
            originalColor = targetText.color;
        }
        else
        {
            Debug.LogWarning(
                gameObject.name + " 上的 TMPColorGlitch 没有找到 TMP_Text。"
            );
        }
    }


    private void OnEnable()
    {
        if (targetText == null)
            return;

        // 记录当前正常颜色
        originalColor = targetText.color;

        glitchCoroutine = StartCoroutine(
            RandomGlitchLoop()
        );
    }


    private void OnDisable()
    {
        if (glitchCoroutine != null)
        {
            StopCoroutine(glitchCoroutine);

            glitchCoroutine = null;
        }
    }


    // ==================================================
    // 持续随机等待并触发 Glitch
    // ==================================================

    private IEnumerator RandomGlitchLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(
                minInterval,
                maxInterval
            );


            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(
                    waitTime
                );
            }
            else
            {
                yield return new WaitForSeconds(
                    waitTime
                );
            }


            yield return StartCoroutine(
                PerformGlitch()
            );
        }
    }


    // ==================================================
    // 执行一次颜色抽动
    // ==================================================

    private IEnumerator PerformGlitch()
    {
        int glitchCount = Random.Range(
            minGlitchCount,
            maxGlitchCount + 1
        );


        for (int i = 0; i < glitchCount; i++)
        {
            Color newColor = GetRandomGlitchColor();


            // 保留当前 Alpha
            // 避免和 Fade 脚本打架
            newColor.a = targetText.color.a;


            // 直接改变颜色
            // 没有任何渐变
            targetText.color = newColor;


            float colorDuration = Random.Range(
                minColorDuration,
                maxColorDuration
            );


            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(
                    colorDuration
                );
            }
            else
            {
                yield return new WaitForSeconds(
                    colorDuration
                );
            }
        }


        // 抽动结束以后恢复正常颜色
        if (returnToOriginalColor)
        {
            Color restoredColor = originalColor;

            // 保留 Fade 当前控制的 Alpha
            restoredColor.a = targetText.color.a;

            targetText.color = restoredColor;
        }
    }


    // ==================================================
    // 获取随机颜色
    // ==================================================

    private Color GetRandomGlitchColor()
    {
        // 完全随机 RGB
        if (useRandomRGB)
        {
            return new Color(
                Random.value,
                Random.value,
                Random.value,
                1f
            );
        }


        // 从指定颜色列表随机选择
        if (glitchColors != null &&
            glitchColors.Length > 0)
        {
            int index = Random.Range(
                0,
                glitchColors.Length
            );

            return glitchColors[index];
        }


        // 没有颜色的话使用原色
        return originalColor;
    }


    // ==================================================
    // 手动触发一次 Glitch
    // ==================================================

    public void TriggerGlitch()
    {
        if (targetText == null)
            return;

        StartCoroutine(
            PerformGlitch()
        );
    }
}