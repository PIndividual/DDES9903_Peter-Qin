using UnityEngine;

public class Bigger_when_closer : MonoBehaviour
{
    public Transform target;
    public float maxScale;//When player goes away, it should be scaled to 50
    public float minScale;//When player is close, it should be scaled to 10
    public float maxDistance;//after player is 10m away, it should no longer scale up
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);//distance between the object and player is here
        float length = Mathf.Clamp01(distance / maxDistance);//the distance devided by the maximum distance, and then clamped between 0 and 1 is how I map the change in size
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, length);//Scale the object based on the length(0-1) within the maxscale and minscale.
    }
}
