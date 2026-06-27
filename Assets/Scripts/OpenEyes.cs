using UnityEngine;
using UnityEngine.UI; // 因为我们要操作UI图片，所以必须引入这个“工具箱”
using System.Collections;

public class WakeUpEffect : MonoBehaviour
{
    [Header("设置")]
    public Image blackScreen;   // 拖入你刚才做的那张纯黑图片
    public float fadeTime = 3f; // 睁眼过程需要花几秒？默认3秒
    public float waitTime = 1f; // 睁眼前等待几秒？默认1秒
    public float targetAlpha = 0f; // 目标透明度，0表示完全透明

    void Start()
    {
        
        if (blackScreen != null)
        {
            StartCoroutine(FadeToClear());
        }
    }

    private IEnumerator FadeToClear()
    {
        // 1. 确保一开始图片是完全不透明的纯黑 (Alpha = 1)
        Color currentColor = blackScreen.color;
        currentColor.a = 1f;
        blackScreen.color = currentColor;
        yield return new WaitForSeconds(waitTime);//https://discussions.unity.com/t/what-exactly-is-the-yield-command/37380
        //https://docs.unity3d.com/6000.3/Documentation/ScriptReference/WaitForSeconds.html

        // 2. 开始计时
        float timer = 0f;

        // 只要经过的时间还没达到你设定的 fadeTime，就一直循环
        while (timer < fadeTime)
        {
            timer += Time.deltaTime; // 累加经过的时间 (每一帧所花费的时间)

            // Mathf.Lerp 是一个平滑过渡的魔法公式
            // 它会让透明度根据时间比例，从 1 平滑过渡到 0
            float alpha = Mathf.Lerp(1f, targetAlpha, timer / fadeTime);

            // 把计算好的透明度应用给图片
            currentColor.a = alpha;
            blackScreen.color = currentColor;

            yield return null;//https://discussions.unity.com/t/in-a-unitytest-what-does-yield-return-null-do-and-when-should-i-use-it/732186/4
        }

    }
}