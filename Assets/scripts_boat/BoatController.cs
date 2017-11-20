using UnityEngine;
using System.Collections;

public class BoatController : MonoBehaviour
{
    //Speed calculations
    private float currentSpeed;
    private Vector3 lastPosition;

    void FixedUpdate()
    {
        CalculateSpeed();

      
    }


    private void CalculateSpeed()
    {
        
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

       
        lastPosition = transform.position;
    }

    public float CurrentSpeed
    {
        get
        {
            return this.currentSpeed;
        }
    }
}