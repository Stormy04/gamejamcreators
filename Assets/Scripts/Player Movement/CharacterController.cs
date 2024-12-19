using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private bool canDash;
    private float dashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                   
                    rb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
                    canDoubleJump = true;
                    canDash = true;
                }
                else if (canDoubleJump)
                {
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
                    canDoubleJump = false;
                }
            }

            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canDash)
            {
                StartDash(moveInput);
                if (!isGrounded)
                {
                    canDash = false;
                }
            }
        }
        else
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }
    }

    private void StartDash(float moveInput)
    {
        isDashing = true;
        dashTime = dashDuration;
        rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDash = true; // Reset dash ability when grounded
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
