using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    private float maxHealth = 40.0f;
    private float currentHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0.0f, maxHealth);
        anim.SetTrigger("trigHit");
        StartCoroutine(GetComponent<PlayerAttack>().CanAttackAfterDamageCoolDown());

        if (currentHealth <= 0.0f)
            Death();

        Debug.Log($"Damage: {currentHealth}/{maxHealth}");
    }

    private void Death()
    {
        anim.SetBool("isDead", true);

        // show game over panel ans such
    }
}
