using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogControls : MonoBehaviour
{
    // Contants that should be initialized from th GUI
    [SerializeField] private float MAX_JUMP_CHARGE = 300.0f;
    [SerializeField] private float JUMP_FORCE = 3000.0f;
    [SerializeField] private float MAX_WALL_HANG_TIME = 5f; // in seconds
    [SerializeField] private float MAX_SPEED_TO_HANG = 7f; // in seconds
    [SerializeField] private LayerMask platformLayerMask;

    // TODO: keep them private (public for debug only)
    public float leftForce = 0.0f;
    public float rightForce = 0.0f;
    public bool isHanging = false;
    public bool hasHanged = false;
    public bool leftReleasedSinceLastJump = true;
    public bool rightReleasedSinceLastJump = true;

    private IEnumerator hangOnWallCoroutine;

    private Rigidbody2D m_RigidBody;
    private BoxCollider2D m_BoxCollider2D;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Jump(Vector2 direction)
    {
        if (leftForce == 0 && rightForce == 0) return;
        Unfreeze();

        float directionAngle = -Vector2.SignedAngle(Vector2.up, direction);
        float angle = directionAngle + (leftForce - rightForce) / (leftForce + rightForce) * 90;
        float force = Mathf.Min(MAX_JUMP_CHARGE, Mathf.Max(leftForce, rightForce)) / MAX_JUMP_CHARGE * JUMP_FORCE;
        float xForce = Mathf.Sin(angle * Mathf.PI / 180.0f) * force;
        float yForce = Mathf.Cos(angle * Mathf.PI / 180.0f) * force;
        Vector2 jumpVector = new Vector2(xForce, yForce);
        m_RigidBody.AddForce(jumpVector);

        ResetJump();

        leftReleasedSinceLastJump = !Input.GetButton("LeftLeg");
        rightReleasedSinceLastJump = !Input.GetButton("RightLeg");
    }

    private void ResetJump()
    {
        // Reset both legs
        leftForce = 0;
        rightForce = 0;
    }

    private bool IsGrounded()
    {
        float groundCheckerHeight = m_BoxCollider2D.bounds.extents.y * 0.1f; // 10% of the collider's height
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_BoxCollider2D.bounds.center, m_BoxCollider2D.bounds.size, 0f, Vector2.down, groundCheckerHeight, platformLayerMask);
        return raycastHit.collider != null;
    }

    private bool IsTouchingRight()
    {
        float leftCheckerWidth = m_BoxCollider2D.bounds.extents.x * 0.1f; // 10% of the collider's width
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_BoxCollider2D.bounds.center, m_BoxCollider2D.bounds.size, 0f, Vector2.right, leftCheckerWidth, platformLayerMask);
        return raycastHit.collider != null;
    }

    private bool IsTouchingLeft()
    {
        float leftCheckerWidth = m_BoxCollider2D.bounds.extents.x * 0.1f; // 10% of the collider's width
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_BoxCollider2D.bounds.center, m_BoxCollider2D.bounds.size, 0f, Vector2.left, leftCheckerWidth, platformLayerMask);
        return raycastHit.collider != null;
    }

    private void Freeze()
    {
        isHanging = true;
        m_RigidBody.constraints |= RigidbodyConstraints2D.FreezePosition;
    }

    private void Unfreeze()
    {
        m_RigidBody.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        m_RigidBody.AddForce(Vector2.zero); // hack to unfreeze position contraints
        isHanging = false;
        hasHanged = !IsGrounded();
    }

    IEnumerator HangOnWall()
    {
        Freeze();
        yield return new WaitForSeconds(MAX_WALL_HANG_TIME);
        Unfreeze();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();
        bool isTouchingLeft = IsTouchingLeft();
        bool isTouchingRight = IsTouchingRight();

        if (isGrounded) hasHanged = false;


        if (!leftReleasedSinceLastJump) leftReleasedSinceLastJump = !Input.GetButton("LeftLeg");
        if (!rightReleasedSinceLastJump) rightReleasedSinceLastJump = !Input.GetButton("RightLeg");

        // Jump and charge logic
        if (
            leftReleasedSinceLastJump
            && rightReleasedSinceLastJump
            && (isGrounded || isTouchingLeft || isTouchingRight)
        ) {
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
                if (isGrounded) Jump(Vector2.up);
                else if (isTouchingRight) Jump(Vector2.left);
                else if (isTouchingLeft) Jump(Vector2.right);
            }
        } else
        {
            ResetJump();
        }

        // Hang logic
        if (!isHanging
            && !hasHanged
            && !isGrounded
            && m_RigidBody.velocity.magnitude < MAX_SPEED_TO_HANG
            && (isTouchingLeft || isTouchingRight)
        ) {
            if (hangOnWallCoroutine != null) StopCoroutine(hangOnWallCoroutine);
            hangOnWallCoroutine = HangOnWall();
            StartCoroutine(hangOnWallCoroutine);
        }
    }
}
