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

    private bool canBlock = true;
    private bool isBlocking = false;
    private float timeTillCanBlock = 0.4f;

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
        if (canBlock && Input.GetKey(KeyCode.Keypad1))
            BlockAttack();
        else
            isBlocking = false;
    }

    public void TakeDamage(float amount)
    {
        anim.SetTrigger("trigHit");
        StartCoroutine(GetComponent<EnemyAttackScript>().CanAttackAfterDamageCoolDown());

        if(isBlocking)
            currentHealth = Mathf.Clamp(currentHealth - DamageAfterBlocking(amount), 0.0f, maxHealth);
        else
            currentHealth = Mathf.Clamp(currentHealth - amount, 0.0f, maxHealth);

        if (currentHealth <= 0.0f)
            Death();

        Debug.Log($"Damaged: {currentHealth} / {maxHealth}");
    }

    private void BlockAttack()
    {
        // set blocking animation

        isBlocking = true;
    }

    private float DamageAfterBlocking(float initialDamage)
    {
        float finalDamage = (20.0f / 100.0f) * initialDamage; // 20 % of initial damage

        return finalDamage;
    }

    private void Death()
    {
        anim.SetBool("isDead", true);
        
        // load game over panel and such
    }
}
