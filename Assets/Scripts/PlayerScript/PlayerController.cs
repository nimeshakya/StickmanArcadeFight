using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private EnemyAttackScript enemyAttackScript;

    private float maxHealth = 40.0f;
    private float currentHealth;

    private bool canBlock = true;
    private bool isBlocking = false;
    public bool _isBlocking { get { return isBlocking; } }
    private float timeTillCanBlock = 0.5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAttackScript = GameObject.FindWithTag("Enemy").GetComponent<EnemyAttackScript>();
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
            StartCoroutine(CanBlockCooldown()); // player is not blocking and takes damage, player must wait till they can block
        }

        anim.SetTrigger("trigHit");
        StartCoroutine(GetComponent<PlayerAttack>().CanAttackAfterDamageCoolDown());

        if (Mathf.Approximately(currentHealth, 0.0f))
            Death();

        Debug.Log($"Damage: {currentHealth}/{maxHealth}");
    }

    private void Death()
    {
        anim.SetBool("isDead", true);

        // show game over panel ans such
    }

    private void BlockAttack()
    {
        // set blocking animation
        if (enemyAttackScript._canAttack)
        {
            isBlocking = true;
        }
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
