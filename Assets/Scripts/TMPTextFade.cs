using UnityEngine;
using TMPro;
using System.Collections;

public class TMPTextFade : MonoBehaviour
{
    [Header("文字组件")]

    [Tooltip("需要进行淡入淡出的 TMP 文字。如果不指定，会自动寻找当前物体上的 TMP_Text。")]
    public TMP_Text targetText;


    [Header("淡入设置")]

    [Tooltip("启用物体时是否自动淡入")]
    public bool fadeInOnEnable = true;

    [Tooltip("淡入开始前等待时间")]
    [Min(0f)]
    public float fadeInDelay = 0f;

    [Tooltip("淡入持续时间（秒）")]
    [Min(0f)]
    public float fadeInTime = 1f;

    [Range(0f, 1f)]
    [Tooltip("淡入结束后的 Alpha")]
    public float fadeInTargetAlpha = 1f;


    [Header("淡出设置")]

    [Tooltip("淡出开始前等待时间")]
    [Min(0f)]
    public float fadeOutDelay = 0f;

    [Tooltip("淡出持续时间（秒）")]
    [Min(0f)]
    public float fadeOutTime = 1f;

    [Range(0f, 1f)]
    [Tooltip("淡出结束后的 Alpha")]
    public float fadeOutTargetAlpha = 0f;

    [Tooltip("淡出完成以后自动 Disable 整个 GameObject")]
    public bool disableAfterFadeOut = true;


    [Header("时间设置")]

    [Tooltip("开启后不受 Time.timeScale 影响")]
    public bool useUnscaledTime = true;


    private Coroutine currentFade;


    private void Awake()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TMP_Text>();
        }

        if (targetText == null)
        {
            Debug.LogWarning(
                gameObject.name + " 上的 TMPTextFade 没有找到 TMP_Text。"
            );
        }
    }


    private void OnEnable()
    {
        if (targetText == null)
            return;

        if (fadeInOnEnable)
        {
            // 每次 Enable 时从完全透明开始
            SetAlpha(0f);

            StartFadeIn();
        }
    }


    // ==================================================
    // 淡入
    // ==================================================

    public void StartFadeIn()
    {
        if (targetText == null)
            return;

        StopCurrentFade();

        currentFade = StartCoroutine(
            FadeRoutine(
                targetText.alpha,
                fadeInTargetAlpha,
                fadeInDelay,
                fadeInTime,
                false
            )
        );
    }


    // ==================================================
    // 从 0 开始淡入
    // ==================================================

    public void FadeInFromZero()
    {
        if (targetText == null)
            return;

        StopCurrentFade();

        SetAlpha(0f);

        currentFade = StartCoroutine(
            FadeRoutine(
                0f,
                fadeInTargetAlpha,
                fadeInDelay,
                fadeInTime,
                false
            )
        );
    }


    // ==================================================
    // 淡出
    // ==================================================

    public void FadeOut()
    {
        if (targetText == null)
            return;

        StopCurrentFade();

        currentFade = StartCoroutine(
            FadeRoutine(
                targetText.alpha,
                fadeOutTargetAlpha,
                fadeOutDelay,
                fadeOutTime,
                disableAfterFadeOut
            )
        );
    }


    // ==================================================
    // 立即显示
    // ==================================================

    public void ShowImmediately()
    {
        StopCurrentFade();

        SetAlpha(fadeInTargetAlpha);
    }


    // ==================================================
    // 立即隐藏
    // ==================================================

    public void HideImmediately()
    {
        StopCurrentFade();

        SetAlpha(fadeOutTargetAlpha);
    }


    // ==================================================
    // Fade 核心
    // ==================================================

    private IEnumerator FadeRoutine(
        float startAlpha,
        float targetAlpha,
        float delay,
        float duration,
        bool disableAtEnd
    )
    {
        // ------------------------------
        // 等待
        // ------------------------------

        if (delay > 0f)
        {
            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
        }


        // ------------------------------
        // 如果时间为 0，立即完成
        // ------------------------------

        if (duration <= 0f)
        {
            SetAlpha(targetAlpha);

            currentFade = null;

            if (disableAtEnd)
            {
                gameObject.SetActive(false);
            }

            yield break;
        }


        // ------------------------------
        // 正常淡入 / 淡出
        // ------------------------------

        float timer = 0f;

        while (timer < duration)
        {
            if (useUnscaledTime)
            {
                timer += Time.unscaledDeltaTime;
            }
            else
            {
                timer += Time.deltaTime;
            }

            float progress = Mathf.Clamp01(
                timer / duration
            );

            float newAlpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                progress
            );

            SetAlpha(newAlpha);

            yield return null;
        }


        // 保证最后精确到达目标值
        SetAlpha(targetAlpha);

        currentFade = null;


        // ------------------------------
        // 淡出以后关闭物体
        // ------------------------------

        if (disableAtEnd)
        {
            gameObject.SetActive(false);
        }
    }


    // ==================================================
    // 修改 Alpha
    // ==================================================

    private void SetAlpha(float alpha)
    {
        targetText.alpha = Mathf.Clamp01(alpha);
    }


    // ==================================================
    // 停止当前 Fade
    // ==================================================

    private void StopCurrentFade()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);

            currentFade = null;
        }
    }
}