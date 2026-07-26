using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Chase_Stop_Chase : MonoBehaviour
{
    public NavMeshAgent AI;
    public Transform target1; // 目标（通常是玩家）

    [Header("Chase and Escape settings")]
    public float stopDistance = 1.5f; // 停下来和玩家保持的距离

    [Header("自定义触发事件")]
    public UnityEvent onArrive; // 替代原来的 Arrive Trigger
    public UnityEvent onLeave;  // 替代原来的 Leave Trigger

    private bool isArrived = false;

    void Start()
    {
        AI = GetComponent<NavMeshAgent>();

        // 自动告诉 NavMeshAgent 靠近目标多少距离时自动刹车停止
        if (AI != null)
        {
            AI.stoppingDistance = stopDistance;
        }
    }

    void Update()
    {
        if (target1 == null || AI == null) return;

        // 一旦激活，就一直更新目标位置
        AI.destination = target1.position;

        // 计算当前距离
        float distanceToTarget = Vector3.Distance(transform.position, target1.position);

        // 如果距离小于设定的停止距离，并且之前还没触发过“到达”
        if (distanceToTarget <= stopDistance && !isArrived)
        {
            onArrive.Invoke(); // 触发你在面板里配置的所有“到达”事件
            isArrived = true;
        }
        // 如果玩家跑远了，并且当前还在“到达”状态
        else if (distanceToTarget > stopDistance && isArrived)
        {
            onLeave.Invoke();  // 触发你在面板里配置的所有“离开”事件
            isArrived = false;
        }
    }
}