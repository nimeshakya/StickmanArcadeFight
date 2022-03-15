using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    [SerializeField] private Transform hitCheckPoint;
    [SerializeField] private float hitCheckRadius;
    [SerializeField] private LayerMask playerLayer;

    private Animator anim;
    private EnemyController enemyController;
    private PlayerController playerController;

    private bool canAttack = true; // if can attack or not (after cooldown)
    public bool _canAttack { get { return canAttack; } }
    private float attackInterval = 0.25f; // interval of each attack
    private float canAttackTimeWhenHit = 0.4f; // time till enemy can counter attack when player gives damage
    private bool isCanAttackCooldownRunning = false; // if can attack cool down running or not

    // different damage for different attack type
    private float swordAttackDamage = 4.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CanAttackCondition();
        AttackPlayer();
    }

    private void CanAttackCondition()
    {
        if (enemyController._isBlocking)
            canAttack = false;
        else
            CanAttackAfterDamageCondition();
    }

    private void CanAttackAfterDamageCondition()
    {
        if (isCanAttackCooldownRunning)
            canAttack = false;
        else
            canAttack = true;
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
        isCanAttackCooldownRunning = true;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
        isCanAttackCooldownRunning = false;
    }

    public IEnumerator CanAttackAfterDamageCoolDown()
    {
        isCanAttackCooldownRunning = true;
        canAttack = false;
        yield return new WaitForSeconds(canAttackTimeWhenHit);
        canAttack = true;
        isCanAttackCooldownRunning = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (hitCheckPoint == null)
            return;

        Gizmos.DrawWireSphere(hitCheckPoint.position, hitCheckRadius);
    }
}
