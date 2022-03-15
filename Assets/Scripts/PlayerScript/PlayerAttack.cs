using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private PlayerController playerController;
    private EnemyController enemyController;

    [SerializeField] private Transform hitCheckPoint;
    [SerializeField] private float hitCheckRadius = 5.0f;
    [SerializeField] private LayerMask enemylayers;

    private bool canAttack = true; // player can attack or not
    public bool _canAttack { get { return canAttack; } set { canAttack = value; } }
    private float attackInterval = 0.25f;
    private float canAttackTimeWhenHit = 0.4f; // time till player can counter attack when enemy gives damage
    private bool isCanAttackCoolDownRunning = false; // if the cann attack cooldown after taking damage running

    // different damage for different attack type
    private float swordAttackDamage = 4.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        enemyController = GameObject.FindWithTag("Enemy").GetComponent<EnemyController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CanAttackCondition();
        AttackEnemy();
    }

    private void CanAttackCondition()
    {
        if (playerController._isBlocking)
            canAttack = false;
        else
            CanAttackAfterDamageContdition();
    }

    private void CanAttackAfterDamageContdition()
    {
        if (isCanAttackCoolDownRunning)
            canAttack = false;
        else
            canAttack = true;
    }

    private void AttackEnemy()
    {
        // multiple keys can be added for various attacks and respective damages
        if (Input.GetKeyDown(KeyCode.X) && canAttack)
        {
            canAttack = false;
            anim.SetTrigger("trigAttack");

            GiveAttackDamage(swordAttackDamage); // give damage of 4.0f

            StartCoroutine(CanAttackCoolDown());
        }
    }

    private void GiveAttackDamage(float amount)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitCheckPoint.position, hitCheckRadius, enemylayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            // if the enemy in enemy layer has tag of "Enemy", give damage
            if (enemy.CompareTag("Enemy"))
                enemyController.TakeDamage(swordAttackDamage);
        }
    }

    IEnumerator CanAttackCoolDown()
    {
        isCanAttackCoolDownRunning = true;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
        isCanAttackCoolDownRunning = false;
    }

    // how long player cannot attack when enemy is attacking (giving damgage) the player
    public IEnumerator CanAttackAfterDamageCoolDown()
    {
        isCanAttackCoolDownRunning = true;
        canAttack = false;
        yield return new WaitForSeconds(canAttackTimeWhenHit);
        canAttack = true;
        isCanAttackCoolDownRunning = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (hitCheckPoint == null)
            return;

        Gizmos.DrawWireSphere(hitCheckPoint.position, hitCheckRadius);
    }
}
