using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField, Range(0.0f, 40.0f)]
    private float m_Speed = 10.0f;
    [SerializeField, Range(1.0f, 5.0f)]
    private float m_SprintSpeed = 1.8f;
    [SerializeField, Range(0.0f, 30.0f)]
    private float m_JumpHeight = 4.3f;
    [SerializeField, Range(-30.0f, 0.0f)]
    private float m_GravityStrength = -20.0f;

    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_Collider;

    private Vector3 m_Velocity = Vector3.zero;

    private bool m_CanJump = true;
    private float m_DistToGround = 0.0f;

    private float m_DefaultSpeed = 0.0f;

    private float horiz = 0.0f;
    private float verti = 0.0f;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.useGravity = false;

        m_Collider = GetComponent<CapsuleCollider>();
        m_DistToGround = m_Collider.bounds.extents.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_DefaultSpeed = m_Speed;
    }

    private void Update()
    {
        if (m_CanJump && Input.GetButton("Jump") && IsGrounded())
        {
            m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2.0f * m_GravityStrength);
            m_CanJump = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Speed = m_DefaultSpeed * m_SprintSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_Speed = m_DefaultSpeed;
        }

        horiz = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = Vector3.ClampMagnitude(transform.right * horiz + transform.forward * verti, 1.0f) * m_Speed;

        m_Velocity.y += m_GravityStrength * Time.fixedDeltaTime;
        m_Velocity = new Vector3(moveDir.x, m_Velocity.y, moveDir.z);

        m_Rigidbody.velocity = m_Velocity;

        if (m_Velocity.y < 0 && IsGrounded())
        {
            m_Velocity.y = 0.0f;
            m_CanJump = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, m_DistToGround + 0.1f);
    }
}
