using UnityEngine;

public class move_up_down : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 sinOffset;
    public float amplitude;
    public float sinValue;
    public float range;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.localPosition;
        

    }

    // Update is called once per frame
    void Update()
    {
        sinValue = Mathf.Sin(amplitude * Mathf.Deg2Rad);
        sinOffset.y = sinValue * range;
        transform.localPosition = startPosition + sinOffset;
        amplitude += speed * Time.deltaTime;
    }
}
