using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private int hp;

    private void Awake()
    {
        //Eventually this will depend on the unit
        hp = 100;
    }
    
    public bool TakeDamage(int damage)
    {
        hp -= damage;

        bool isDead = hp <= 0;

        if (isDead)
        {
            Die();
        }

        return isDead;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
