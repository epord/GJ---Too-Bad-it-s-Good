using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogControls : MonoBehaviour
{
    // Contants that should be initialized from th GUI
    public float MAX_JUMP_CHARGE = 300.0f;
    public float JUMP_FORCE = 3000.0f;

    // TODO: keep them private (public for debug only)
    public float leftForce = 0.0f;
    public float rightForce = 0.0f;

    private Rigidbody2D m_RigidBody;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Jump()
    {
        if (leftForce == 0 && rightForce == 0) return;

        float angle = (leftForce - rightForce) / (leftForce + rightForce) * 90;
        float force = Mathf.Max(leftForce, rightForce) / MAX_JUMP_CHARGE * JUMP_FORCE;
        float xForce = Mathf.Sin(angle * Mathf.PI / 180.0f) * force;
        float yForce = Mathf.Cos(angle * Mathf.PI / 180.0f) * force;
        Vector2 jumpVector = new Vector2(xForce, yForce);
        m_RigidBody.AddForce(jumpVector);

        // Reset both legs
        leftForce = 0;
        rightForce = 0;
    }

    void Update()
    {

        // TODO: do not allow to charge nor jump when in the air

        if (Input.GetButton("LeftLeg"))
        {
            leftForce += 1.0f;
        }

        if (Input.GetButton("RightLeg"))
        {
            rightForce += 1.0f;
        }

        if (Input.GetButtonUp("LeftLeg") || Input.GetButtonUp("RightLeg")
            || rightForce >= MAX_JUMP_CHARGE || leftForce >= MAX_JUMP_CHARGE)
        {
            Jump();
        }

    }
}
