using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private float currentSpeed;
    public float jumpForce = 10.0f;
    public float fallMultiplier = 5.5f;
    public float lowJumpMultiplier = 3f;

    [SerializeField] private Transform groundCheckObject; // object to check for ground
    [SerializeField] private float groundedRadius = 5.0f; // radius up to where ground is checked
    [SerializeField] private LayerMask groundLayers; // all ground layers

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;
    private SpriteRenderer sr;
    private Animator anim;

    private float movementX;
    private bool jump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveInput();
        PlayerAnimations(IsGrounded());
        LockPlayerMoveInAir();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        DirectedJumpPlayer();
        JumpPlayer();
        MultiplyFall();
    }

    void PlayerMoveInput()
    {
        movementX = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void MovePlayer()
    {
        rb.velocity = new Vector2(movementX * currentSpeed, rb.velocity.y);
    }

    // jump player veritcally
    void JumpPlayer()
    {
        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
    }

    // jump player in a direction
    void DirectedJumpPlayer()
    {
        if (jump && movementX != 0)
        {
            Debug.Log("Directed Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
            while (!IsGrounded())
            {
                rb.velocity = new Vector2(movementX * moveSpeed, rb.velocity.y); // movementX multiplied by movespeed instead of current speed 'cause current speed is 0 in air
            }
        }
    }

    // fast fall
    void MultiplyFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        // check collision with ground
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheckObject.position, groundedRadius, groundLayers);

        // if there is any collisions with "ground" from ground layer, return true
        if (groundColliders.Length > 0)
            return true;
        else
            return false;
    }

    private void PlayerAnimations(bool isGrounded)
    {
        if(movementX > 0 || movementX < 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (!isGrounded)
            anim.SetBool("isJumping", true);
        else
            anim.SetBool("isJumping", false);

        if (!isGrounded && rb.velocity.y < 0)
            anim.SetBool("isFalling", true);
        else
            anim.SetBool("isFalling", false);
    }

    void LockPlayerMoveInAir()
    {
        if (IsGrounded())
            currentSpeed = moveSpeed;
        else
            currentSpeed = 0.0f;
    }

    private void OnDrawGizmosSelected()
    {
        // don't do anything if ground check object is not assigned (or is null)
        if (groundCheckObject == null)
            return;

        Gizmos.DrawWireSphere(groundCheckObject.position, groundedRadius);
    }
}
