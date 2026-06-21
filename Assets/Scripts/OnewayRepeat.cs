using UnityEngine;

public class SimpleTraffic : MonoBehaviour
{
    [Header("行驶设置")]
    [Tooltip("车辆行驶的速度")]
    public float speed = 10f;

    [Tooltip("车辆行驶多远后传送回起点")]
    public float travelDistance = 30f;

    // 记录车辆在游戏开始时的初始位置
    private Vector3 startPosition;

    void Start()
    {
        // 游戏运行时，记下这台车当前所在的坐标
        startPosition = transform.position;
    }

    void Update()
    {
        // 让车辆沿着它自己的正前方（Z轴）匀速移动
        // Space.Self 确保它是按照车头所指的方向开，而不是世界坐标的方向
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        // 计算车辆当前位置和初始位置之间的距离
        float currentDistance = Vector3.Distance(startPosition, transform.position);

        // 如果开出的距离超过了我们设定的最大距离
        if (currentDistance >= travelDistance)
        {
            // 直接把车“传送”回初始位置
            transform.position = startPosition;
        }
    }
}