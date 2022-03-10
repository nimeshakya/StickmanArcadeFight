using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

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

            // call function to do damage and stuff

            StartCoroutine(CanAttackCoolDown());
        }
    }

    IEnumerator CanAttackCoolDown()
    {
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }
}
