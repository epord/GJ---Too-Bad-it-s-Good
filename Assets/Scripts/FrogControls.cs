using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogControls : MonoBehaviour
{
    // Contants that should be initialized from th GUI
    [SerializeField] private float MAX_JUMP_CHARGE = 300.0f;
    [SerializeField] private float JUMP_FORCE = 3000.0f;
    [SerializeField] private LayerMask platformLayerMask;

    // TODO: keep them private (public for debug only)
    public float leftForce = 0.0f;
    public float rightForce = 0.0f;

    private Rigidbody2D m_RigidBody;
    private BoxCollider2D m_BoxCollider2D;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Jump()
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

    private bool IsGrounded()
    {
        float groundCheckerHeight = m_BoxCollider2D.bounds.extents.y * 0.1f; // 10% of the collider's height
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_BoxCollider2D.bounds.center, m_BoxCollider2D.bounds.size, 0f, Vector2.down, groundCheckerHeight, platformLayerMask);
        Debug.DrawRay(m_BoxCollider2D.bounds.center + new Vector3(m_BoxCollider2D.bounds.extents.x, 0), Vector2.down * (m_BoxCollider2D.bounds.extents.y + groundCheckerHeight), Color.green);
        Debug.DrawRay(m_BoxCollider2D.bounds.center - new Vector3(m_BoxCollider2D.bounds.extents.x, 0), Vector2.down * (m_BoxCollider2D.bounds.extents.y + groundCheckerHeight), Color.green);
        Debug.DrawRay(m_BoxCollider2D.bounds.center - new Vector3(0, m_BoxCollider2D.bounds.extents.y), Vector2.right * (m_BoxCollider2D.bounds.extents.y), Color.green);
        return raycastHit.collider != null;
    }

    void Update()
    {
        if (IsGrounded())
        {
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
}
