using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Movement variables
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
    private float originalJumpForce;
    public bool isOnSkateboard; // Add this variable
    public Transform skateboard; // Add a reference to the skateboard
    #endregion

    private IThrowObject throwObjectBehaviour;
    private Vector3 initialPosition;
    public Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalJumpForce = jumpForce;
        initialPosition = transform.position;
        throwObjectBehaviour = GetComponent<IThrowObject>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            if (moveInput > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (!isOnSkateboard)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded)
                    {
                        rb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
                        canDoubleJump = true;
                        canDash = true;
                        animator.SetBool("IsJumping", true);
                    }
                    else if (canDoubleJump)
                    {
                        rb.velocity = Vector2.zero;
                        rb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
                        canDoubleJump = false;
                        animator.SetBool("IsJumping", true);
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Jump"))
                {
                    isOnSkateboard = false;
                    foreach (Transform child in transform)
                    {
                        child.parent = null;
                        child.GetComponent<Rigidbody2D>().isKinematic = false;
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
        }
        else
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ThrowObject();
        }

        if (transform.position.y < -10)
        {
            transform.position = initialPosition;
        }
    }

    private void StartDash(float moveInput)
    {
        isDashing = true;
        dashTime = dashDuration;
        rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
    }

    private void ThrowObject()
    {
        throwObjectBehaviour.ThrowObject();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDash = true;
            animator.SetBool("IsJumping", false);
        }
        else if (collision.gameObject.CompareTag("Bed"))
        {
            isGrounded = true;
            jumpForce *= 1.5f;
            animator.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else if (collision.gameObject.CompareTag("Bed"))
        {
            jumpForce = originalJumpForce;
            isGrounded = false;
        }
    }

    public void MountSkateboard(Transform skateboardTransform)
    {
        isOnSkateboard = true;
        skateboardTransform.parent = transform;
        transform.position = new Vector3(skateboardTransform.position.x, skateboardTransform.position.y + 0.75f, skateboardTransform.position.z);
    }
}
