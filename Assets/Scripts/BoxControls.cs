using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BoxControls : MonoBehaviour
{
    // Contants that should be initialized from th GUI
    public float MAX_JUMP_CHARGE = 300.0f;
    public float BASE_CHARGE = 5.0f;
    public float JUMP_FORCE = 3000.0f;
    public float TORQUE_FORCE = 10.0f;
    public float AngularSpeed;
    public float Speed;
    public int DOUBLE_JUMP_WINDOW_MILISECONDS = 1000;
    public AudioSource BottomLeftSound;
    public AudioSource TopLeftSound;
    public AudioSource BottomRightSound;
    public AudioSource TopRightSound;

    
    private Rigidbody2D m_RigidBody;
    private Corner bottomLeftCorner; 
    private Corner topLeftCorner; 
    private Corner bottomRightCorner; 
    private Corner topRightCorner;
    private Corner[] corners;
    private DateTime lastTimeJumped;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();

        SpriteRenderer yellowSprite = transform.Find("yellow").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer redSprite = transform.Find("red").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer blueSprite = transform.Find("blue").gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer greenSprite = transform.Find("green").gameObject.GetComponent<SpriteRenderer>();

        bottomLeftCorner = new Corner(transform.Find("BottomLeftCorner"), "bottomLeftJump", yellowSprite, BottomLeftSound);
        topLeftCorner = new Corner(transform.Find("TopLeftCorner"), "topLeftJump", redSprite, TopLeftSound);
        bottomRightCorner = new Corner(transform.Find("BottomRightCorner"), "bottomRightJump", blueSprite, BottomRightSound);
        topRightCorner = new Corner(transform.Find("TopRightCorner"), "topRightJump", greenSprite, TopRightSound);

        corners = new Corner[]{bottomLeftCorner, topLeftCorner, bottomRightCorner, topRightCorner};
    }

    public class Corner
    {
        public float charge = 0.0f;
        public Transform transform;
        public string buttonName;
        public SpriteRenderer sprite;
        public AudioSource jumpSound;
        public Corner(Transform transform, String buttonName, SpriteRenderer sprite, AudioSource jumpSound)
        {
            this.transform = transform;
            this.buttonName = buttonName;
            this.sprite = sprite;
            this.jumpSound = jumpSound;
        }
    }

    void Jump(Corner corner)
    {

    
        if (corner.charge < 0.1) return;
        
        DateTime currentTime = DateTime.Now;
        if ((currentTime - lastTimeJumped).Milliseconds >= DOUBLE_JUMP_WINDOW_MILISECONDS)
        {
            m_RigidBody.velocity = Vector3.zero;
        }
        lastTimeJumped = currentTime;

        
        // Force        
        float absoluteForce = (BASE_CHARGE + corner.charge) / MAX_JUMP_CHARGE * JUMP_FORCE;
        Vector3 forceVector = Vector3.Normalize(gameObject.transform.position - corner.transform.position);
        Vector3 force = forceVector * absoluteForce;
        Vector3 position = corner.transform.position;
        m_RigidBody.AddForceAtPosition(force, position);

        // Rotation force

        // We multiply with forceVector.x to make less rotation when x is smaller. It also changes the rotation from
        // clockwise to counter clockwise if x is negative.
        float torque = (BASE_CHARGE + corner.charge) * forceVector.x * TORQUE_FORCE * -1; 
        m_RigidBody.AddTorque(torque);
      
        // Reset
        corner.charge = 0.0f;
        corner.jumpSound.Play();
       
    }


    void Update()
    {
        AngularSpeed = m_RigidBody.angularVelocity;
        Speed = m_RigidBody.velocity.magnitude;
        foreach(Corner corner in corners)
        {
            if (Input.GetButtonDown(corner.buttonName))
            {
                corner.charge += 1.0f;
            }
            else if (corner.charge > 0 && Input.GetButton(corner.buttonName))
            {
                corner.charge += 1.0f;
                corner.sprite.enabled = true;
            }
            if (Input.GetButtonUp(corner.buttonName) ||  corner.charge >= MAX_JUMP_CHARGE - BASE_CHARGE)
            {
                Jump(corner);
                corner.sprite.enabled = false;
            }
        }
    }
}
