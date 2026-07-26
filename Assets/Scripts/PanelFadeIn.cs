using UnityEngine;

public class PanelFadeIn : MonoBehaviour
{
    [Header("动画设置")]
    public float animDuration = 0.5f;
    public Vector3 startOffset = new Vector3(0, -0.5f, 0);

    private Vector3 targetPosition;
    private float timer = 0f;
    private bool isAnimating = false; // 改成了 isAnimating，因为现在不做 Fade 了

    void Awake()
    {
        // 记录物体在场景里正确的终点位置
        targetPosition = transform.position;
    }

    void OnEnable()
    {
        timer = 0f;
        // 激活时，瞬间把它拉到设定的偏移位置（默认在下方 0.5 处）
        transform.position = targetPosition + startOffset;
        isAnimating = true;
    }

    void Update()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;

            if (timer < animDuration)
            {
                // 计算进度与平滑曲线
                float progress = timer / animDuration;
                float curve = progress * progress * (3f - 2f * progress);

                // 只做位移插值，不管透明度了
                transform.position = Vector3.Lerp(targetPosition + startOffset, targetPosition, curve);
            }
            else
            {
                // 动画结束，强制锁定到终点位置
                transform.position = targetPosition;
                isAnimating = false;
            }
        }
    }
}