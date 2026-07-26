using UnityEngine;
using UnityEngine.Events;

public class RunOnce : MonoBehaviour
{
    [Header("只会执行一次的事件")]
    public UnityEvent actionsToDoOnce;

    // 这是一个内部的锁，用来记录是否已经触发过
    private bool hasExecuted = false;

    // 这个方法就是让别人（比如你的追逐脚本）来 Invoke 调用的
    public void TriggerAction()
    {
        // 只有在没执行过的情况下，才会放行
        if (!hasExecuted)
        {
            actionsToDoOnce.Invoke(); // 执行你在面板里配置的所有功能
            hasExecuted = true;       // 上锁，以后再叫也不理了
        }
    }

    // 附加一个重置功能，以防你以后需要在某些特定情况下（比如玩家重新加载关卡）再次触发它
    public void ResetExecution()
    {
        hasExecuted = false;
    }
}