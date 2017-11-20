using UnityEngine;
using System.Collections;
public class BoatEngine : MonoBehaviour
    
{
    float speed = 0f;
    float acceleration = 1.0f;
    float maxspeed = 5.0f;
    float minspeed = 0f;
    float heading = 0.0f;
    float rudder = 0.0f;
    float rudderDelta = 15.0f;
    float maxRudder = 180.0f;
    float bob = 0.1f;
    float bobFrequency = 0.2f;
    public float tempSecondes = 5f;

    private float elapsed = 0.0f;
    private float seaLevel = 0.0f;
    private GameObject rudderControl;
    private float rudderAngle = 0.0f;

    float signedSqrt(float x)
    {
        float r = Mathf.Sqrt(Mathf.Abs(x));
        if (x < 0)
        {
            return -r;
        }
        else
        {
            return r;
        }
    }

    void Update()
    {
        Debug.Log("Sailing script activated");
        //padadam
        elapsed += Time.deltaTime;
        float tempY = seaLevel + bob * Mathf.Sin(elapsed * bobFrequency * (Mathf.PI * 2));
        transform.position = new Vector3(transform.position.x, tempY, transform.position.z);

        // zataceni
        rudder += Input.GetAxis("Horizontal") * rudderDelta * Time.deltaTime;
        if (rudder > maxRudder)
        {
            rudder = -maxRudder;
        }
        else if (rudder < -maxRudder)
        {
            rudder = maxRudder;
        }
        heading = (heading + rudder * Time.deltaTime * signedSqrt(speed)) % 360;
 
        //ovladani v y
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, heading, transform.eulerAngles.z);
        //ovladani v z
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -rudder, transform.eulerAngles.z);

        if (rudderControl != null)
        {
            rudderAngle = ((-60 * rudder) / maxRudder + heading) % 360;
            //ovladani kormidla
            rudderControl.transform.eulerAngles = new Vector3(0, rudderAngle, 0);
        }

        //  plavba
        if (Input.GetKey(KeyCode.W)) 
        { speed += 1f * Time.deltaTime; }
          
        if (speed > maxspeed)
        {
            speed = maxspeed;
        }
        //kotva


        if (Input.GetKey(KeyCode.K))

          
            
                {
        
            speed = 0;
         

        }


else if (speed < minspeed)
        {
            speed = minspeed;
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void Awake()
    {
        seaLevel = transform.position.y;
        rudderControl = GameObject.Find("rudderControl");
    }
}
//©Akamiru