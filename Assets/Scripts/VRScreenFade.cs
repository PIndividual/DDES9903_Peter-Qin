using UnityEngine;
using System.Collections;

public class VRScreenFade : MonoBehaviour
{
    // 游戏开始时的 Fade 类型
    public enum StartFadeMode
    {
        None,
        FromBlack,
        FromWhite
    }


    [Header("基础设置")]

    [Tooltip("负责覆盖玩家视野的 Sphere Renderer")]
    public Renderer fadeRenderer;


    [Header("游戏开始")]

    [Tooltip("游戏开始时自动执行哪一种 Fade")]
    public StartFadeMode startFadeMode = StartFadeMode.FromBlack;


    [Header("睁眼 / 淡入设置")]

    [Tooltip("睁眼开始前等待多久")]
    public float openWaitTime = 1f;

    [Tooltip("睁眼过程持续多久")]
    public float openFadeTime = 3f;

    [Range(0f, 1f)]
    [Tooltip("睁眼结束后的不透明度。0 = 完全透明，1 = 完全遮住")]
    public float openTargetAlpha = 0f;


    [Header("闭眼 / 淡出设置")]

    [Tooltip("闭眼开始前等待多久")]
    public float closeWaitTime = 0f;

    [Tooltip("闭眼过程持续多久")]
    public float closeFadeTime = 2f;

    [Range(0f, 1f)]
    [Tooltip("闭眼结束后的不透明度。通常设为 1")]
    public float closeTargetAlpha = 1f;


    private Material fadeMaterial;
    private Coroutine currentFade;


    private void Awake()
    {
        // 如果 Inspector 没有手动指定 Renderer，
        // 就尝试获取这个 GameObject 自己的 Renderer。
        if (fadeRenderer == null)
        {
            fadeRenderer = GetComponent<Renderer>();
        }

        if (fadeRenderer != null)
        {
            // 创建独立 Material 实例，
            // 避免修改 Project 里的原始 Material。
            fadeMaterial = fadeRenderer.material;
        }
        else
        {
            Debug.LogWarning("VRScreenFade 找不到 Renderer！");
        }
    }


    private void Start()
    {
        switch (startFadeMode)
        {
            case StartFadeMode.FromBlack:
                FadeFromBlack();
                break;

            case StartFadeMode.FromWhite:
                FadeFromWhite();
                break;

            case StartFadeMode.None:
                break;
        }
    }


    // ==================================================
    // 黑色 -> 场景
    // ==================================================

    public void FadeFromBlack()
    {
        StartNewFade(
            Color.black,
            1f,
            openTargetAlpha,
            openWaitTime,
            openFadeTime
        );
    }


    // ==================================================
    // 白色 -> 场景
    // ==================================================

    public void FadeFromWhite()
    {
        StartNewFade(
            Color.white,
            1f,
            openTargetAlpha,
            openWaitTime,
            openFadeTime
        );
    }


    // ==================================================
    // 场景 -> 黑色
    // ==================================================

    public void FadeToBlack()
    {
        StartNewFade(
            Color.black,
            GetCurrentAlpha(),
            closeTargetAlpha,
            closeWaitTime,
            closeFadeTime
        );
    }


    // ==================================================
    // 场景 -> 白色
    // ==================================================

    public void FadeToWhite()
    {
        StartNewFade(
            Color.white,
            GetCurrentAlpha(),
            closeTargetAlpha,
            closeWaitTime,
            closeFadeTime
        );
    }


    // ==================================================
    // 立即完全透明
    // ==================================================

    public void ClearFade()
    {
        if (fadeMaterial == null)
            return;

        StopCurrentFade();

        Color color = fadeMaterial.color;
        color.a = 0f;
        fadeMaterial.color = color;
    }


    // ==================================================
    // 启动 Fade
    // ==================================================

    private void StartNewFade(
        Color fadeColor,
        float startAlpha,
        float targetAlpha,
        float waitTime,
        float fadeTime
    )
    {
        if (fadeMaterial == null)
            return;

        StopCurrentFade();

        currentFade = StartCoroutine(
            FadeRoutine(
                fadeColor,
                startAlpha,
                targetAlpha,
                waitTime,
                fadeTime
            )
        );
    }


    // ==================================================
    // Fade 主逻辑
    // ==================================================

    private IEnumerator FadeRoutine(
        Color fadeColor,
        float startAlpha,
        float targetAlpha,
        float waitTime,
        float fadeTime
    )
    {
        // 先设置 Fade 的颜色以及开始 Alpha
        Color color = fadeColor;
        color.a = startAlpha;
        fadeMaterial.color = color;


        // 等待
        if (waitTime > 0f)
        {
            yield return new WaitForSecondsRealtime(waitTime);
        }


        // 如果 Fade 时间为 0，
        // 就直接跳到最终状态。
        if (fadeTime <= 0f)
        {
            color.a = targetAlpha;
            fadeMaterial.color = color;

            currentFade = null;
            yield break;
        }


        float timer = 0f;


        while (timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                timer / fadeTime
            );


            // 让 Fade 开头和结尾更柔和
            progress = Mathf.SmoothStep(
                0f,
                1f,
                progress
            );


            float alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                progress
            );


            color.a = alpha;
            fadeMaterial.color = color;


            yield return null;
        }


        // 确保最终精确到达目标值
        color.a = targetAlpha;
        fadeMaterial.color = color;


        currentFade = null;
    }


    // ==================================================
    // 获取当前 Alpha
    // ==================================================

    private float GetCurrentAlpha()
    {
        if (fadeMaterial == null)
            return 0f;

        return fadeMaterial.color.a;
    }


    // ==================================================
    // 停止正在进行的 Fade
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