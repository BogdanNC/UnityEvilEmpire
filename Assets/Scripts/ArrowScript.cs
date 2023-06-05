using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private int baseDmg = 10;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.collider.gameObject;

        CombatManager targetCM = target.GetComponent<CombatManager>();

        if(targetCM == null)
        {
            Debug.Log("Hit wrong target! Abort!");
            Destroy(gameObject);
            return;
        }

        targetCM.TakeDamage(baseDmg);

        Destroy(gameObject);
    }
}
