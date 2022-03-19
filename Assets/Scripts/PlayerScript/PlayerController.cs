using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Transform enemyTransform;

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

    private bool isDead; // if character is dead or not
    public bool _isDead { get { return isDead; } set { isDead = value; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyTransform = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        _isDead = false;
    }

    private void Update()
    {
        LookDirection(); // makes player look towards enemy according to position of the enemy
        if (canBlock && Input.GetKey(KeyCode.B))
        {
            BlockAttack();
        } else
        {
            isBlocking = false;
        }
    }

    // function to makes player look towards enemy according to position of the enemy
    private void LookDirection()
    {
        float directionScale = Mathf.Sign(enemyTransform.position.x - transform.position.x);

        transform.localScale = new Vector2(directionScale, 1);
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

        PlayerHealthBarUI.instance.SetValue(currentHealth / maxHealth); // change UI health bar
    }

    private void Death()
    {
        _isDead = true;
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
