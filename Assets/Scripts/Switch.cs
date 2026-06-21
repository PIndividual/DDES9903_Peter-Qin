using UnityEngine;

public class Switch : MonoBehaviour
{
    [Tooltip("把有需求的Parent拖到这里")]
    public GameObject BatchParent;

    // 这是一个公开方法，供玩家交互时调用
    public void Toggle()
    {
        // 先检查是否已经正确拖拽了父物体，防止报错
        if (BatchParent != null)
        {
            // BatchParent.activeSelf 会返回该物体当前是显示(true)还是隐藏(false)
            // ! 是取反逻辑。也就是：如果当前开着，就关掉；如果关着，就打开。
            BatchParent.SetActive(!BatchParent.activeSelf);
        }
        else
        {
            Debug.LogWarning("父物体未指定！请在 Inspector 中拖拽赋值。");
        }
    }
}