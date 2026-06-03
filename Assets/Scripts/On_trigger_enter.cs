using UnityEngine;
using UnityEngine.Events;

public class On_trigger_enter : MonoBehaviour
{
    public UnityEvent Condition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by " + other.name);
        Condition.Invoke();
    }
}
