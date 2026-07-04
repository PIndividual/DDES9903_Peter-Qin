using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SequentialActivation : MonoBehaviour
{
    [Header("序列事件设置")]
    public UnityEvent[] sequentialEvents; 

    [Tooltip("每次执行之间的时间间隔（秒）")]
    public float spawnDelay = 0.15f;

    public void StartSequence() 
    {
        // 开启协程
        StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        // 遍历数组中的每一个事件块
        foreach (UnityEvent thingsToDo in sequentialEvents)
        {
            // 确保该事件块不是空的
            if (thingsToDo != null)
            {
                // 【核心】调用 Invoke()，相当于让脚本帮你依次“点击”了 Inspector 里的每一个事件
                thingsToDo.Invoke();

                // 等待设定的时间间隔
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}