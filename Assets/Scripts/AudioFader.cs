using UnityEngine;
using System.Collections;

public class AudioFader : MonoBehaviour
{

    public float fadeDuration = 1.5f;

    [Tooltip("勾选：淡出并停止后，悄悄恢复音量（适合以后还要再次播放的声音）。\n取消勾选：淡出并停止后，音量永远保持为0。")]
    public bool restoreOriginalVolume = true;

    // 这个方法专门暴露给你的 UnityEvent (图形化列表) 调用
    public void FadeOutAudio(AudioSource audioToFade)
    {
        if (audioToFade != null && audioToFade.isPlaying)
        {
            // 开启协程，让音量随着时间慢慢减小
            StartCoroutine(FadeRoutine(audioToFade));
        }
    }

    private IEnumerator FadeRoutine(AudioSource audioToFade)
    {
        // 记录音频当前的初始音量
        float startVolume = audioToFade.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // 线性平滑插值，把音量从初始值慢慢降到0
            audioToFade.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            // 等待下一帧继续执行
            yield return null;
        }

        // 动画结束，确保音量死死锁定在 0
        audioToFade.volume = 0f;

        // 彻底停止音频播放
        audioToFade.Stop();

        // 【核心修改】：根据你的面板选项，决定是否恢复音量
        if (restoreOriginalVolume)
        {
            audioToFade.volume = startVolume;
        }
    }
}