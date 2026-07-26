using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [Header("New Skybox Material")]
    public Material newSkyboxMaterial;

    // 这个方法用于换上新的天空盒
    public void ChangeSkybox()
    {
        if (newSkyboxMaterial != null)
        {
            RenderSettings.skybox = newSkyboxMaterial;
        }
    }

    // 这个方法用于彻底关闭天空盒（背景会变成摄像机设置的纯色）
    public void TurnOffSkybox()
    {
        RenderSettings.skybox = null;
    }
}