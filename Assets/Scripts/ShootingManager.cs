using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    private void Shoot(Transform target)
    {
        GameObject newArrow = Instantiate(arrow, transform.position, Quaternion.identity);

        newArrow.transform.LookAt(target);
    }
}
