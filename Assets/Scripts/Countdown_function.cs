using UnityEngine;
using UnityEngine.Events;

public class Countdown_function : MonoBehaviour
{
    public float countdownTime; // Total countdown time in seconds
    private float timer;
    public bool isActivated;
    public UnityEvent onCountdownFinished;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            timer += Time.deltaTime;
            if (timer >= countdownTime)
            {
                
                onCountdownFinished.Invoke();
                timer = 0f; // Reset timer if you want to repeat the countdown
                isActivated = false;
            }
        }
        //Debug.Log(timer);
        //Debug.Log(isActivated);
    }
     public void ActivateCountdown()
    {
        isActivated = true;
        //timer = 0f; // Reset timer when activated
    }
}
