using UnityEngine;
using UnityEngine.Events; // 必须引入这个命名空间才能使用 UnityEvent

public class Password : MonoBehaviour
{
    [Header("密码设定 (例如: 0=左, 1=上, 2=下, 3=右)")]
    [Tooltip("在这里按顺序填入正确的数字")]
    public int[] correctPassword = { 0, 1, 2, 3 };

    // 记录玩家当前输入到了第几位
    private int currentStep = 0;

    [Header("UI 元素引用")]
    [Tooltip("把那四个星号物体按顺序拖进来")]
    public GameObject[] asterisks;

    [Header("自定义结果事件 (在面板上自由配置)")]
    [Tooltip("当四位密码全部输入正确时触发")]
    public UnityEvent onPasswordSuccess;

    [Tooltip("当密码输入错误任何一位时立刻触发")]
    public UnityEvent onPasswordFail;

    void Start()
    {
        // 游戏开始时，确保所有星号都是隐藏的
        ResetPassword();
    }

    /// <summary>
    /// 当玩家按下方向键时调用
    /// </summary>
    public void OnDirectionButtonPressed(int inputValue)
    {
        // 检查玩家输入的数字，是否等于当前步骤所需的正确数字
        if (inputValue == correctPassword[currentStep])
        {
            // 输入正确！显示对应的星号
            if (currentStep < asterisks.Length)
            {
                asterisks[currentStep].SetActive(true);
            }

            // 步骤加一
            currentStep++;

            // 检查是否四个键都输完了
            if (currentStep >= correctPassword.Length)
            {
                // 密码完全正确！调用成功事件列表里的所有功能
                onPasswordSuccess.Invoke();

                ResetPassword();
            }
        }
        else
        {
            // 输入错误！调用失败事件列表里的所有功能
            onPasswordFail.Invoke();

            ResetPassword();
        }
    }

    // 重置进度和星号显示
    public void ResetPassword()
    {
        currentStep = 0;
        foreach (GameObject asterisk in asterisks)
        {
            if (asterisk != null)
            {
                asterisk.SetActive(false);
            }
        }
    }
}