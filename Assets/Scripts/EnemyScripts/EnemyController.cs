using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private float maxHealth = 40.0f;
    public float currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        anim.SetTrigger("trigHit");
        currentHealth = Mathf.Clamp(currentHealth - amount, 0.0f, maxHealth);

        if (currentHealth <= 0.0f)
            Death();

        Debug.Log($"Damaged: {currentHealth} / {maxHealth}");
    }

    private void Death()
    {
        anim.SetBool("isDead", true);
        
        // load game over panel and such
    }
}
