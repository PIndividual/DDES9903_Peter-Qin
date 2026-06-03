using UnityEngine;
using UnityEngine.Events;

public class Play_Sound_else_On_Hit : MonoBehaviour
{
    private AudioSource audioSource;
    public UnityEvent OtherThingsToDo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bump detected");
        OtherThingsToDo.Invoke();

        if (audioSource != null && audioSource.clip !=null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing on " + gameObject.name);
        }
        
    }
}
