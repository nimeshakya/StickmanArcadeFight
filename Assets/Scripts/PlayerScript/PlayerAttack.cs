using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform hitCheckPoint;
    [SerializeField] private float hitCheckRadius = 5.0f;
    [SerializeField] private LayerMask enemylayers;

    private bool canAttack = true; // player can attack or not

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackInput();
    }

    private void AttackInput()
    {
        // multiple keys can be added for various attacks and respective damages
        if (Input.GetKeyDown(KeyCode.X) && canAttack)
        {
            canAttack = false;
            anim.SetTrigger("trigAttack");

            GiveAttackDamage(4.0f); // give damage of 4.0f

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
                enemy.GetComponent<EnemyController>().TakeDamage(amount);
        }
    }

    IEnumerator CanAttackCoolDown()
    {
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (hitCheckPoint == null)
            return;

        Gizmos.DrawWireSphere(hitCheckPoint.position, hitCheckRadius);
    }
}
