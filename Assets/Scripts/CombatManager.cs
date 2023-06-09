using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private int hp;
    public int maxHP;

    public GameObject hpBar;

    private void Awake()
    {
        //Eventually this will depend on the unit
        hp = 100;
        maxHP = hp;
    }
    
    public bool TakeDamage(int damage)
    {
        hp -= damage;

        float newValue = (2 / maxHP) * hp;

        hpBar.transform.localScale = new Vector3(newValue, 1, 1);

        bool isDead = (hp <= 0);

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
