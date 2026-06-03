using UnityEngine;

public class SwapBetweenSound : MonoBehaviour
{
    public AudioSource music1;
    public AudioSource music2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

public void SwapMusic()
    {
        if (music1.isPlaying)
        {
            music1.Stop();
            music2.Play();
        }
        else
        {
            music2.Stop();
            music1.Play();
        }
}

// Update is called once per frame
void Update()
    {
        
    }
}
