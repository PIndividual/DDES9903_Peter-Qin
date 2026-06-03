using UnityEngine;
// This script is written based on the spin script
public class Height_Adjust : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float heightOffset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //To change the height of an item, we need to modify its y position based on the gizmo in my scene 
        transform.Translate(0,heightOffset * Time.deltaTime,0);
    }

    public void setHeightOffset(float newHeightOffset)
    {
        heightOffset = newHeightOffset;
    }
    public void Stop()// If I don't put this here, there won't be a way to stop the change in height once the function is triggered by the button
    {
        heightOffset = 0;

    }
}
