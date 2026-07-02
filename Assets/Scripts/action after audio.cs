using UnityEngine;
using UnityEngine.Events; // 引入 UnityEvent 需要这个命名空间
using System.Collections;

public class PlayAndTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource myAudioSource;  // 用来播放声音的组件
    public AudioClip myMonologue;      // 你要播放的独白音频文件

    [Header("Action After Playback")]
    public UnityEvent onAudioFinished; // 在 Inspector 面板里可视化配置的自定义事件

    // 这个方法可以被场景里的触发器（比如你走进某个区域）调用
    public void StartPlaying()
    {
        // 开启协程，开始播放并计时的流程
        StartCoroutine(PlayAndWaitRoutine()); //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/MonoBehaviour.StartCoroutine.html
    }

    private IEnumerator PlayAndWaitRoutine()
    {
        // 1. 把音频文件赋给 AudioSource，然后播放
        if (myAudioSource != null && myMonologue != null)
        {
            myAudioSource.clip = myMonologue;
            myAudioSource.Play();

            // 2. 核心逻辑：等待这个音频的长度（秒）
            yield return new WaitForSeconds(myMonologue.length); //https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
        }
        else
        {
            Debug.LogWarning("Audio source or audio clip not configured!");
            yield break; // 如果没配置就直接退出
        }

        // 3. 等待结束后，执行你在 Inspector 里配置的所有操作
        onAudioFinished.Invoke();
    }
}