using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BoxControls : MonoBehaviour
{
    // Contants that should be initialized from th GUI
    public float MAX_JUMP_CHARGE = 300.0f;
    public float JUMP_FORCE = 3000.0f;
    public float TORQUE_FORCE = 10.0f;
    public float AngularSpeed;
    public float Speed; 

    private Rigidbody2D m_RigidBody;
    private Corner bottomLeftCorner; 
    private Corner topLeftCorner; 
    private Corner bottomRightCorner; 
    private Corner topRightCorner;
    private Corner[] corners;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        bottomLeftCorner = new Corner(transform.Find("BottomLeftCorner"), "bottomLeftJump");
        topLeftCorner = new Corner(transform.Find("TopLeftCorner"), "topLeftJump");
        bottomRightCorner = new Corner(transform.Find("BottomRightCorner"), "bottomRightJump");
        topRightCorner = new Corner(transform.Find("TopRightCorner"), "topRightJump");
        corners = new Corner[]{bottomLeftCorner, topLeftCorner, bottomRightCorner, topRightCorner};
    }

    public class Corner
    {
        public float force = 0.0f;
        public Transform transform;
        public string buttonName;
        public Corner(Transform transform, String buttonName)
        {
            this.transform = transform;
            this.buttonName = buttonName;
        }
    }

    void Jump(Corner corner)
    {
        
        
        if (corner.force < 0.1) return;
        
        // Force        
        float absoluteForce = corner.force / MAX_JUMP_CHARGE * JUMP_FORCE;
        Vector3 forceVector = Vector3.Normalize(gameObject.transform.position - corner.transform.position);
        Vector3 force = forceVector * absoluteForce;
        Vector3 position = corner.transform.position;
        m_RigidBody.AddForceAtPosition(force, position);

        // Rotation force

        // We multiply with forceVector.x to make less rotation when x is smaller. It also changes the rotation from
        // clockwise to counter clockwise if x is negative.
        float torque = corner.force * forceVector.x * TORQUE_FORCE * -1; 
        m_RigidBody.AddTorque(torque);
      
        // Reset
        corner.force = 0.0f;
       
    }

    void Update()
    {
        AngularSpeed = m_RigidBody.angularVelocity;
        Speed = m_RigidBody.velocity.magnitude;
        foreach(Corner corner in corners)
        {
            if (Input.GetButton(corner.buttonName))
            {
                corner.force += 1.0f;
            }
            if (Input.GetButtonUp(corner.buttonName) ||  corner.force >= MAX_JUMP_CHARGE)
            {
                Jump(corner);
            }
        }
    }
}
