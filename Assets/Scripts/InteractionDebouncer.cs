using UnityEngine;
using UnityEngine.Events;

public class InteractionDebouncer : MonoBehaviour
{
    [Header("防止重复触发")]

    [Tooltip("两次有效触发之间至少需要间隔多少秒")]
    [Min(0f)]
    public float cooldownTime = 0.2f;


    [Header("通过防抖后执行的事件")]

    [Tooltip("只有有效触发才会执行这里的事件")]
    public UnityEvent onAcceptedTrigger;


    private float lastTriggerTime = Mathf.NegativeInfinity;


    // ==================================================
    // 给按钮调用这个方法
    // ==================================================

    public void TryTrigger()
    {
        float currentTime = Time.unscaledTime;

        // 距离上一次有效触发太近
        // 直接忽略
        if (currentTime - lastTriggerTime < cooldownTime)
        {
            return;
        }


        // 记录这次有效触发的时间
        lastTriggerTime = currentTime;


        // 真正执行按钮事件
        onAcceptedTrigger?.Invoke();
    }


    // ==================================================
    // 如有需要，可以手动重置
    // ==================================================

    public void ResetDebounce()
    {
        lastTriggerTime = Mathf.NegativeInfinity;
    }
}