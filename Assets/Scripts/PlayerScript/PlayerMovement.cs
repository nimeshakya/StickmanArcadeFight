using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
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
    private bool isGrounded;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveInput();
        PlayerAnimations(IsGrounded());
    }

    private void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
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
        Vector2 position = transform.position;
        if (movementX > 0 || movementX < 0)
            position.x += movementX * moveSpeed * Time.deltaTime;

        transform.position = position;
    }

    void JumpPlayer()
    {
        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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

    private void OnDrawGizmosSelected()
    {
        // don't do anything if ground check object is not assigned (or is null)
        if (groundCheckObject == null)
            return;

        Gizmos.DrawWireSphere(groundCheckObject.position, groundedRadius);
    }
}
