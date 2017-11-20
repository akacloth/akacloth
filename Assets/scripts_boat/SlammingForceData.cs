using UnityEngine;
using System.Collections;



public class SlammingForceData : MonoBehaviour
{
    //The area of the original triangles - calculate once in the beginning because always the same
    public float originalArea;
    //How much area of a triangle in the whole boat is submerged
    public float submergedArea;
    //Same as above but previous time step
    public float previousSubmergedArea;
    //Need to save the center of the triangle to calculate the velocity
    public Vector3 triangleCenter;
    //Velocity
    public Vector3 velocity;
    //Same as above but previous time step
    public Vector3 previousVelocity;
}