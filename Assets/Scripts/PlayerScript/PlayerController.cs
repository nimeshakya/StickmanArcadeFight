using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private CapsuleCollider2D damageCheckCollider;
    [SerializeField] private CapsuleCollider2D characterBlockerCollider;
    [SerializeField] private CapsuleCollider2D enemyCollider;
    [SerializeField] private CapsuleCollider2D enemyDamageCheckCollider;
    [SerializeField] private CapsuleCollider2D enemyCharacterBlockerCollider;

    private float maxHealth = 40.0f;
    private float currentHealth;

    private bool canBlock = true;
    private bool isBlocking = false;
    public bool _isBlocking { get { return isBlocking; } }
    private float timeTillCanBlock = 0.5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (canBlock && Input.GetKey(KeyCode.B))
        {
            BlockAttack();
        } else
        {
            isBlocking = false;
        }
    }

    public void TakeDamage(float amount)
    {
        if(isBlocking)
            currentHealth = Mathf.Clamp(currentHealth - DamageAfterBlocking(amount), 0.0f, maxHealth);
        else
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0.0f, maxHealth);
            StopCoroutine(CanBlockCooldown()); // stop previous cooldown time
            StartCoroutine(CanBlockCooldown()); // if player is not blocking and takes damage, player must wait till they can block
        }

        anim.SetTrigger("trigHit");
        StopCoroutine(GetComponent<PlayerAttack>().CanAttackAfterDamageCoolDown());
        StartCoroutine(GetComponent<PlayerAttack>().CanAttackAfterDamageCoolDown());

        if (Mathf.Approximately(currentHealth, 0.0f))
            Death();

        Debug.Log($"Damage: {currentHealth}/{maxHealth}");
    }

    private void Death()
    {
        anim.SetBool("isDead", true);

        // show game over panel ans such

        // ignore collision with player after death
        damageCheckCollider.enabled = false;
        characterBlockerCollider.enabled = false;
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), enemyCollider);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), enemyDamageCheckCollider);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), enemyCharacterBlockerCollider);

        // disable all script when dead
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<BlockCharacterCollision>().enabled = false;
        this.enabled = false;
    }

    private void BlockAttack()
    {
        // set blocking animation

        isBlocking = true;
    }

    private float DamageAfterBlocking(float initialDamage)
    {
        float finalDamage = (20.0f / 100.0f) * initialDamage; // 20% of initial damage
        Debug.Log(finalDamage);
        return finalDamage;
    }

    // time till player can block after taking damage
    private IEnumerator CanBlockCooldown()
    {
        canBlock = false;
        yield return new WaitForSeconds(timeTillCanBlock);
        canBlock = true;
    }
}
