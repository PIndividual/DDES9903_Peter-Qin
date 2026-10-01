using UnityEngine;
using System.Collections;

public class VRScreenFade : MonoBehaviour
{
    // ==================================================
    // 游戏开始时的 Fade 类型
    // ==================================================

    public enum StartFadeMode
    {
        None,
        FromBlack,
        FromWhite
    }


    // ==================================================
    // Inspector 设置
    // ==================================================

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


    [Header("完全睁眼设置")]

    [Tooltip("完全睁眼开始前等待多久")]
    public float fullyOpenWaitTime = 0f;

    [Tooltip("从当前 Alpha 完全淡到透明需要多久")]
    public float fullyOpenFadeTime = 2f;


    [Header("闭眼 / 淡出设置")]

    [Tooltip("闭眼开始前等待多久")]
    public float closeWaitTime = 0f;

    [Tooltip("闭眼过程持续多久")]
    public float closeFadeTime = 2f;

    [Range(0f, 1f)]
    [Tooltip("闭眼结束后的不透明度。通常设为 1")]
    public float closeTargetAlpha = 1f;


    // ==================================================
    // 内部变量
    // ==================================================

    private Material fadeMaterial;

    private Coroutine currentFade;


    // ==================================================
    // Awake
    // ==================================================

    private void Awake()
    {
        // 如果 Inspector 没有手动指定 Renderer，
        // 就自动获取当前 GameObject 上的 Renderer。
        if (fadeRenderer == null)
        {
            fadeRenderer = GetComponent<Renderer>();
        }


        if (fadeRenderer != null)
        {
            // 创建一个独立的 Material 实例。
            // 这样运行时修改颜色和 Alpha
            // 不会影响 Project 里的原始材质。
            fadeMaterial = fadeRenderer.material;
        }
        else
        {
            Debug.LogWarning(
                "VRScreenFade 找不到 Renderer！请把 VR Fade Sphere 的 Mesh Renderer 拖到 Fade Renderer。"
            );
        }
    }


    // ==================================================
    // Start
    // ==================================================

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
    // 从当前状态继续完全睁眼
    // 当前 Alpha -> 0
    // ==================================================

    public void FadeToClear()
    {
        if (fadeMaterial == null)
            return;


        Color currentColor = fadeMaterial.color;


        StartNewFade(
            currentColor,
            currentColor.a,
            0f,
            fullyOpenWaitTime,
            fullyOpenFadeTime
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
    // 启动新的 Fade
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
        // --------------------------------------------------
        // 设置开始颜色和 Alpha
        // --------------------------------------------------

        Color color = fadeColor;

        color.a = startAlpha;

        fadeMaterial.color = color;


        // --------------------------------------------------
        // Fade 前等待
        // --------------------------------------------------

        if (waitTime > 0f)
        {
            yield return new WaitForSecondsRealtime(waitTime);
        }


        // --------------------------------------------------
        // 如果 Fade 时间为 0
        // 直接跳到最终状态
        // --------------------------------------------------

        if (fadeTime <= 0f)
        {
            color.a = targetAlpha;

            fadeMaterial.color = color;

            currentFade = null;

            yield break;
        }


        // --------------------------------------------------
        // 正式开始 Fade
        // --------------------------------------------------

        float timer = 0f;


        while (timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;


            float progress = Mathf.Clamp01(
                timer / fadeTime
            );


            // SmoothStep 让开头和结尾更柔和
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


        // --------------------------------------------------
        // 确保最终 Alpha 精确到目标值
        // --------------------------------------------------

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
    // 停止当前正在进行的 Fade
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