
using UnityEngine;

public class DecoupledGlitcher : MonoBehaviour
{
    [Header("0. 随机时间控制 (Random Timing)")]
    [Tooltip("最小更新间隔（秒），决定了最快能闪多快")]
    public float minUpdateInterval = 0.02f; 
    [Tooltip("最大更新间隔（秒），决定了最长能卡顿多久")]
    public float maxUpdateInterval = 0.25f; 
    
    private float timer = 0f;
    // 新增：记录当前这一轮需要等待的具体时间
    private float currentTargetInterval;

    [Header("1. 位移崩坏设置 (Position Glitch)")]
    [Range(0f, 1f)] public float positionProbability = 0.05f;
    public float maxDisplacement = 0.2f;

    [Header("2. 旋转崩坏设置 (Rotation Glitch)")]
    [Range(0f, 1f)] public float rotationProbability = 0.05f;
    [Tooltip("单次旋转摇摆的最大角度(度)")]
    public float maxRotationAngle = 20f;

    [Header("3. 闪烁崩坏设置 (Flicker Glitch)")]
    [Range(0f, 1f)] public float flickerProbability = 0.05f;

    // 记录初始状态
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private MeshRenderer meshRenderer;

    void Start()
    {
        // 记录物体初始的本地位置和旋转
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        meshRenderer = GetComponent<MeshRenderer>();

        // 游戏开始时，先在区间内随机抽取第一个等待时间
        currentTargetInterval = Random.Range(minUpdateInterval, maxUpdateInterval);
    }

    void Update()
    {

        // Time.deltaTime 是上一帧到这一帧所花费的时间（通常是 0.016 秒左右）
        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);
        Debug.Log("Next Glitch: " + currentTargetInterval);
        // 2. 判断累加的时间是否达到了我们设定的“间隔阈值”
        if (timer >= currentTargetInterval)
        {
            // 3. 达到时间后，清零计时器，准备下一轮计时
            timer = 0f;
            currentTargetInterval = Random.Range(minUpdateInterval, maxUpdateInterval);
            // 模块一：独立控制位移
            if (Random.value < positionProbability)
            {
                Vector3 randomOffset = Random.insideUnitSphere * maxDisplacement;
                transform.localPosition = originalPosition + randomOffset;
            }
            else
            {
                transform.localPosition = originalPosition;
            }

            // 模块二：独立控制旋转
            if (Random.value < rotationProbability)
            {
                // 在X、Y、Z三个轴上分别生成随机旋转角度
                float randomX = Random.Range(-maxRotationAngle, maxRotationAngle);
                float randomY = Random.Range(-maxRotationAngle, maxRotationAngle);
                float randomZ = Random.Range(-maxRotationAngle, maxRotationAngle);

                // 将欧拉角转换为四元数，并叠加到初始旋转上
                Quaternion randomRot = Quaternion.Euler(randomX, randomY, randomZ);
                transform.localRotation = originalRotation * randomRot;
            }
            else
            {
                transform.localRotation = originalRotation;
            }

            // 模块三：独立控制闪烁 (显隐)
            if (meshRenderer != null)
            {
                if (Random.value < flickerProbability)
                {
                    // 触发故障时，以50%的概率决定这一帧是亮还是灭，制造高频闪烁感
                    // 如果随机数大于 0.5
                    if (Random.value > 0.5f)
                    {
                        // 显示模型（开启）
                        meshRenderer.enabled = true;
                    }
                    else
                    {
                        // 隐藏模型（关闭）
                        meshRenderer.enabled = false;
                    }
                }
                else
                {
                    // 未触发故障时，保持正常显示
                    meshRenderer.enabled = true;
                }
            }
        }
    }
}