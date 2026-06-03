using UnityEngine;

public class After_Player : MonoBehaviour
{
    public Transform target;
    public float followDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position - target.forward * followDistance;
    }
}
