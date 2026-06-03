using UnityEngine;

public class SimplyQUIT : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit button pressed. Exiting...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //https://discussions.unity.com/t/start-stop-playmode-from-editor-script/27701/3
        #else
            Application.Quit();
        #endif
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
