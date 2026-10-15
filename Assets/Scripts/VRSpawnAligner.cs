
using System.Collections;
using UnityEngine;

public class VRSpawnAligner : MonoBehaviour
{
    [Header("VR Player References")]
    [Tooltip("整个 XR 玩家根物体")]
    [SerializeField] private Transform playerRoot;

    [Tooltip("玩家实际使用的 VR Camera")]
    [SerializeField] private Transform headCamera;

    [Header("Spawn Settings")]
    [Tooltip("是否让玩家面朝出生点的正前方")]
    [SerializeField] private bool alignFacingDirection = true;

    [Tooltip("场景加载后等待多少帧再执行归位")]
    [SerializeField, Range(0, 10)]
    private int waitFrames = 2;

    private IEnumerator Start()
    {
        if (playerRoot == null || headCamera == null)
        {
            Debug.LogError(
                "[VRSpawnAligner] 请指定 Player Root 和 Head Camera！",
                this
            );
            yield break;
        }

        // 等待 XR 相机更新追踪位置
        for (int i = 0; i < waitFrames; i++)
        {
            yield return null;
        }

        RepositionPlayer();
    }

    public void RepositionPlayer()
    {
        if (playerRoot == null || headCamera == null)
            return;

        // 暂时关闭玩家的 CharacterController，
        // 避免移动时与碰撞检测发生冲突
        CharacterController[] controllers =
            playerRoot.GetComponentsInChildren<CharacterController>(true);

        bool[] enabledStates = new bool[controllers.Length];

        for (int i = 0; i < controllers.Length; i++)
        {
            enabledStates[i] = controllers[i].enabled;
            controllers[i].enabled = false;
        }

        // 1. 对齐玩家朝向
        if (alignFacingDirection)
        {
            Vector3 currentForward =
                Vector3.ProjectOnPlane(headCamera.forward, Vector3.up);

            Vector3 targetForward =
                Vector3.ProjectOnPlane(transform.forward, Vector3.up);

            if (currentForward.sqrMagnitude > 0.001f &&
                targetForward.sqrMagnitude > 0.001f)
            {
                float angle = Vector3.SignedAngle(
                    currentForward,
                    targetForward,
                    Vector3.up
                );

                // 围绕头显而不是 Rig 原点旋转
                playerRoot.RotateAround(
                    headCamera.position,
                    Vector3.up,
                    angle
                );
            }
        }

        // 2. 计算头显与出生点之间的水平偏移
        Vector3 offset = transform.position - headCamera.position;
        offset.y = 0f;

        // 3. 移动整个玩家 Rig，补偿真实头显的偏移
        playerRoot.position += offset;

        // 4. 将 Rig 的地面高度对齐出生点
        // 出生点应放在玩家脚下的地面高度
        Vector3 position = playerRoot.position;
        position.y = transform.position.y;
        playerRoot.position = position;

        // 更新物理变换
        Physics.SyncTransforms();

        // 恢复 CharacterController 原来的启用状态
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].enabled = enabledStates[i];
        }

        Debug.Log(
            "[VRSpawnAligner] 玩家已自动对齐至出生点：" + gameObject.name,
            this
        );
    }
}
