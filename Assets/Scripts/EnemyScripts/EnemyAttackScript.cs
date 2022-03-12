using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    [SerializeField] private Transform hitCheckPoint;
    [SerializeField] private float hitCheckRadius;
    [SerializeField] private LayerMask playerLayer;

    private Animator anim;
    private PlayerController playerController;

    private bool canAttack = true; // if can attack or not (after cooldown)
    private float attackInterval = 0.25f;

    // different damage for different attack type
    private float swordAttackDamage = 4.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackPlayer();
    }
    
    void AttackPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) && canAttack)
        {
            canAttack = false;
            anim.SetTrigger("trigAttack");

            GiveAttackDamage(swordAttackDamage);

            StartCoroutine(CanAttackCoolDown());
        }
    }

    void GiveAttackDamage(float amount)
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(hitCheckPoint.position, hitCheckRadius, playerLayer);

        foreach(Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                playerController.TakeDamage(amount);
            }
        }
    }

    IEnumerator CanAttackCoolDown()
    {
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (hitCheckPoint == null)
            return;

        Gizmos.DrawWireSphere(hitCheckPoint.position, hitCheckRadius);
    }
}
