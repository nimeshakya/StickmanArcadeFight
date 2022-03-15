using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private CapsuleCollider2D damageCheckCollider;
    [SerializeField] private CapsuleCollider2D characterBlockerCollider;
    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private CapsuleCollider2D playerDamageCheckCollider;
    [SerializeField] private CapsuleCollider2D playerCharacterBlockerCollider;
 
    private float maxHealth = 40.0f;
    private float currentHealth;

    private bool canBlock = true;
    private bool isBlocking = false;
    public bool _isBlocking { get { return isBlocking; } }
    private float timeTillCanBlock = 0.5f;

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
        if(isBlocking)
            currentHealth = Mathf.Clamp(currentHealth - DamageAfterBlocking(amount), 0.0f, maxHealth);
        else
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0.0f, maxHealth);

            StopCoroutine(CanBlockCooldown());
            StartCoroutine(CanBlockCooldown()); // start cooldown till player can block after taking damage
        }

        anim.SetTrigger("trigHit");

        StopCoroutine(GetComponent<EnemyAttackScript>().CanAttackAfterDamageCoolDown());
        StartCoroutine(GetComponent<EnemyAttackScript>().CanAttackAfterDamageCoolDown());

        if (Mathf.Approximately(currentHealth, 0.0f))
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
        Debug.Log(finalDamage);
        return finalDamage;
    }

    private void Death()
    {
        anim.SetBool("isDead", true);

        // load game over panel and such

        // ignore collision with player after death
        damageCheckCollider.enabled = false;
        characterBlockerCollider.enabled = false;
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), playerCollider);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), playerDamageCheckCollider);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), playerCharacterBlockerCollider);


        // disable damage checking after death
        damageCheckCollider.enabled = false;

        // disable all scripts
        GetComponent<EnemyAttackScript>().enabled = false;
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<BlockCharacterCollision>().enabled = false;
        this.enabled = false;
    }

    private IEnumerator CanBlockCooldown()
    {
        canBlock = false;
        yield return new WaitForSeconds(timeTillCanBlock);
        canBlock = true;
    }
}
