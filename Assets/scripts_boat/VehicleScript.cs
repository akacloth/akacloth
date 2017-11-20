using UnityEngine;
using System.Collections;

public class VehicleScript : MonoBehaviour
{
    public float speed = 10;
    public float rotationspeed = 50;

    // Use this for initialization
    // Update is called once per frame
    void Update()
    {

        // Forward movement
        if (Input.GetKey(KeyCode.I))
            transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Backward movement
        if (Input.GetKey(KeyCode.K))
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Left movement
        if (Input.GetKey(KeyCode.J))
            transform.Rotate(Vector3.down * rotationspeed * Time.deltaTime);

        // Right movement
        if (Input.GetKey(KeyCode.L))
            transform.Rotate(Vector3.up * rotationspeed * Time.deltaTime);

    }
}