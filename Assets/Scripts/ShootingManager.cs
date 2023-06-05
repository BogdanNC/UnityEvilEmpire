using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float launchVel = 20.0f;
    [SerializeField] private float attackRange = 20.0f;

    [SerializeField] private float attackCooldown = 2.0f;
    private float waitTime = 0.0f;
    private bool canShoot = true;

    [SerializeField] private bool isAlly;

    private GameObject target = null;

    private void Update()
    {
        if(target == null || !InRange(target.transform))
        {
            //Target was destroyed or walked out of range
            target = FindTarget();

            if (!target)
            {
                return;
            }
        }

        Debug.Log("Target: " + target.name);

        if (canShoot)
        {
            Debug.Log("Can shoot!");
            if((isAlly && target.CompareTag("Enemy")))
            {
                Debug.Log("Shooting!");
                Shoot(target.transform);
                canShoot = false;
            }
        }
        else
        {
            Debug.Log("On Cooldown");
            waitTime += Time.deltaTime;

            if(waitTime >= attackCooldown)
            {
                canShoot = true;
                waitTime = 0;
            }
        }
    }

    private GameObject FindTarget()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, attackRange);

        foreach(Collider collider in inRange)
        {
            bool allegiance1 = (collider.gameObject.CompareTag("Enemy") && isAlly);
            bool allegiance2 = (collider.gameObject.CompareTag("Ally") && !isAlly);

            bool isValid = allegiance1 || allegiance2;


            if (isValid)
            {
                return collider.gameObject;
            }
        }

        return null;
    }

    private bool InRange(Transform target)
    {
        return ( Vector3.Distance(transform.position, target.transform.position) <= attackRange );
    }

    private void Shoot(Transform target)
    {
        GameObject newArrow = Instantiate(arrow, transform.position, Quaternion.identity);

        newArrow.transform.LookAt(target);

        Vector3 direction = (target.position - transform.position).normalized;

        newArrow.GetComponent<Rigidbody>().velocity = direction * launchVel;
    }
}
