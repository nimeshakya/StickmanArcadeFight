using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 10.0f;
    public float fallMultiplier = 5.5f;
    public float lowJumpMultiplier = 3f;

    [SerializeField] private LayerMask jumpLayerMask;

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

        // play jump animation with respect to character going up or down in air
        if(rb.velocity.y > 0)
            anim.SetTrigger("trigJump");
        else if (rb.velocity.y < 0)
            anim.SetBool("isFalling", true);

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
        float extraheightTest = 5f;
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size,CapsuleDirection2D.Vertical ,0f ,Vector2.down, extraheightTest, jumpLayerMask);

        Debug.Log(raycastHit.collider.name);

        return raycastHit.collider != null;
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

        if (isGrounded)
            anim.SetBool("isFalling", false);
    }
}
