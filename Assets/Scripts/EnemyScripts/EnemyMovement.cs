using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayers;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveSpeed = 10.0f;
    private float movementX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        PlayerAnimation();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void GetMovementInput()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.J))
            movementX = -1;
        else if (IsGrounded() && Input.GetKey(KeyCode.L))
            movementX = 1;
        else
            movementX = 0;
    }

    private void MoveEnemy()
    {
        rb.velocity = new Vector2(movementX * moveSpeed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPoint.position, groundCheckRadius, groundLayers);

        return colliders != null;
    }

    private void PlayerAnimation()
    {
        if(movementX != 0)
        {
            anim.SetBool("isRunning", true);
        } else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
            return;

        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
