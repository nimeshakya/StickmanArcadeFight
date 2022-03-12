using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    private float maxHealth = 40.0f;
    private float currenthealth;
    public float _currentHealth { get; private set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0.0f, maxHealth);
        Debug.Log($"Damage: {_currentHealth}/{maxHealth}");
    }
}
