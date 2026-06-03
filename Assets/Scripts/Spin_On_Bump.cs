using UnityEngine;

public class Spin_On_Bump : MonoBehaviour
{
    public Rigidbody rigid;
    public float xspinForce;
    public float yspinForce;
    public float zspinForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Bump detected");
        rigid.AddTorque(xspinForce, yspinForce, zspinForce);
    }
}
