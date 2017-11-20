using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BoatPhysics : MonoBehaviour
{
  
    public GameObject boatMeshObj;
    public GameObject underWaterObj;
    public GameObject aboveWaterObj;

   
    public Vector3 centerOfMass;


    private ModifyBoatMesh modifyBoatMesh;

   
    private Mesh underWaterMesh;
    private Mesh aboveWaterMesh;

    
    private Rigidbody boatRB;

  
    private float rhoWater = BoatPhysicsMath.RHO_OCEAN_WATER;
    private float rhoAir = BoatPhysicsMath.RHO_AIR;

    void Awake()
    {
        boatRB = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
       
        modifyBoatMesh = new ModifyBoatMesh(boatMeshObj, underWaterObj, aboveWaterObj, boatRB);

       
        underWaterMesh = underWaterObj.GetComponent<MeshFilter>().mesh;
        aboveWaterMesh = aboveWaterObj.GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
       
        modifyBoatMesh.GenerateUnderwaterMesh();

       
        modifyBoatMesh.DisplayMesh(underWaterMesh, "UnderWater Mesh", modifyBoatMesh.underWaterTriangleData);

        //Display the above water mesh
        //modifyBoatMesh.DisplayMesh(aboveWaterMesh, "AboveWater Mesh", modifyBoatMesh.aboveWaterTriangleData);
    }

    void FixedUpdate()
    {
        //Change the center of mass - experimental - move to Start() later
        boatRB.centerOfMass = centerOfMass;

        //Add forces to the part of the boat that's below the water
        if (modifyBoatMesh.underWaterTriangleData.Count > 0)
        {
            AddUnderWaterForces();
        }

        //Add forces to the part of the boat that's above the water
        if (modifyBoatMesh.aboveWaterTriangleData.Count > 0)
        {
            AddAboveWaterForces();
        }
    }

    //Add all forces that act on the squares below the water
    void AddUnderWaterForces()
    {
        //The resistance coefficient - same for all triangles
        float Cf = BoatPhysicsMath.ResistanceCoefficient(
            rhoWater,
            boatRB.velocity.magnitude,
            modifyBoatMesh.CalculateUnderWaterLength());

        //To calculate the slamming force we need the velocity at each of the original triangles
        List<SlammingForceData> slammingForceData = modifyBoatMesh.slammingForceData;

        CalculateSlammingVelocities(slammingForceData);

        //Need this data for slamming forces
        float boatArea = modifyBoatMesh.boatArea;
        float boatMass = 400; //Replace this line with your boat's total mass

        //To connect the submerged triangles with the original triangles
        List<int> indexOfOriginalTriangle = modifyBoatMesh.indexOfOriginalTriangle;

        //Get all triangles
        List<TriangleData> underWaterTriangleData = modifyBoatMesh.underWaterTriangleData;

        for (int i = 0; i < underWaterTriangleData.Count; i++)
        {
            TriangleData triangleData = underWaterTriangleData[i];


            //Calculate the forces
            Vector3 forceToAdd = Vector3.zero;

            //Force 1 - The hydrostatic force (buoyancy)
            forceToAdd += BoatPhysicsMath.BuoyancyForce(rhoWater, triangleData);

            //Force 2 - Viscous Water Resistance
            forceToAdd += BoatPhysicsMath.ViscousWaterResistanceForce(rhoWater, triangleData, Cf);

            //Force 3 - Pressure drag
            forceToAdd += BoatPhysicsMath.PressureDragForce(triangleData);

            //Force 4 - Slamming force
            //Which of the original triangles is this triangle a part of
            int originalTriangleIndex = indexOfOriginalTriangle[i];

            SlammingForceData slammingData = slammingForceData[originalTriangleIndex];

            forceToAdd += BoatPhysicsMath.SlammingForce(slammingData, triangleData, boatArea, boatMass);


            //Add the forces to the boat
            boatRB.AddForceAtPosition(forceToAdd, triangleData.center);


            //Debug

            //Normal
            //Debug.DrawRay(triangleData.center, triangleData.normal * 3f, Color.white);

            //Buoyancy
            //Debug.DrawRay(triangleData.center, BoatPhysicsMath.BuoyancyForce(rhoWater, triangleData).normalized * -3f, Color.blue);

            //Velocity
            //Debug.DrawRay(triangleCenter, triangleVelocityDir * 3f, Color.black);

            //Viscous Water Resistance
            //Debug.DrawRay(triangleCenter, viscousWaterResistanceForce.normalized * 3f, Color.black);
        }
    }



    //Add all forces that act on the squares above the water
    void AddAboveWaterForces()
    {
        //Get all triangles
        List<TriangleData> aboveWaterTriangleData = modifyBoatMesh.aboveWaterTriangleData;

        //Loop through all triangles
        for (int i = 0; i < aboveWaterTriangleData.Count; i++)
        {
            TriangleData triangleData = aboveWaterTriangleData[i];


            //Calculate the forces
            Vector3 forceToAdd = Vector3.zero;

            //Force 1 - Air resistance 
            //Replace VisbyData.C_r with your boat's drag coefficient
            forceToAdd += BoatPhysicsMath.AirResistanceForce(rhoAir, triangleData, 400);

            //Add the forces to the boat
            boatRB.AddForceAtPosition(forceToAdd, triangleData.center);


            //Debug

            //The normal
            //Debug.DrawRay(triangleCenter, triangleNormal * 3f, Color.white);

            //The velocity
            //Debug.DrawRay(triangleCenter, triangleVelocityDir * 3f, Color.black);

            if (triangleData.cosTheta > 0f)
            {
                //Debug.DrawRay(triangleCenter, triangleVelocityDir * 3f, Color.black);
            }

            //Air resistance
            //-3 to show it in the opposite direction to see what's going on
            //Debug.DrawRay(triangleCenter, airResistanceForce.normalized * -3f, Color.blue);
        }
    }



    //Calculate the current velocity at the center of each triangle of the original boat mesh
    private void CalculateSlammingVelocities(List<SlammingForceData> slammingForceData)
    {
        for (int i = 0; i < slammingForceData.Count; i++)
        {
            //Set the new velocity to the old velocity
            slammingForceData[i].previousVelocity = slammingForceData[i].velocity;

            //Center of the triangle in world space
            Vector3 center = transform.TransformPoint(slammingForceData[i].triangleCenter);

            //Get the current velocity at the center of the triangle
            slammingForceData[i].velocity = BoatPhysicsMath.GetTriangleVelocity(boatRB, center);
        }
    }

}