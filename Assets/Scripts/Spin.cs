using UnityEngine;

public class Spin : MonoBehaviour
{
    public float yspeed;
    public float xspeed;
    public float zspeed;
    public float xacceleration;
    public float zacceleration;
    public float yacceleration;
    public float maxSpeeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(xspeed) < maxSpeeed && Mathf.Abs(yspeed) < maxSpeeed && Mathf.Abs(zspeed)< maxSpeeed) {
        xspeed += xacceleration * Time.deltaTime;
        yspeed += yacceleration * Time.deltaTime;
        zspeed += zacceleration * Time.deltaTime;
        }
        else if (Mathf.Abs(xspeed)>maxSpeeed || Mathf.Abs(yspeed)>maxSpeeed || Mathf.Abs(zspeed)>maxSpeeed) {
            xacceleration = 0;
            yacceleration = 0;
            zacceleration = 0;
        } 
    
        transform.Rotate(xspeed * Time.deltaTime, yspeed * Time.deltaTime, zspeed*Time.deltaTime);//fixes issue caused by different framerates

    }

    public void Reverse()
    {
        xspeed = xspeed * -1;
        yspeed = yspeed * -1;
        zspeed = zspeed * -1;
        xacceleration = xacceleration * -1;
        yacceleration = yacceleration * -1;
        zacceleration = zacceleration * -1;
    }
    public void Stop()
    {
        xspeed = 0;
        yspeed = 0;
        zspeed = 0;
        xacceleration = 0;
        yacceleration = 0;
        zacceleration = 0;
    }

    //public void setSpeed(float xnewSpeed, float ynewSpeed, float znewSpeed)
    public void setSpeedY (float ynewSpeed)
    {
        //xspeed = xnewSpeed;
        yspeed = ynewSpeed;
        //zspeed = znewSpeed;
    }
    public void setSpeedX(float xnewSpeed)
    {
        xspeed = xnewSpeed;
        //yspeed = ynewSpeed;
        //zspeed = znewSpeed;
    }

    public void setSpeedZ(float znewSpeed)
    {
        //xspeed = xnewSpeed;
        //yspeed = ynewSpeed;
        zspeed = znewSpeed;
    }

    public void setAccelerationX(float xnewAcceleration)
    {
        xacceleration = xnewAcceleration;
    }
    public void setAccelerationY(float ynewAcceleration)
    {
        yacceleration = ynewAcceleration;
    }
    public void setAccelerationZ(float znewAcceleration)
    {
        zacceleration = znewAcceleration;
    }

}
