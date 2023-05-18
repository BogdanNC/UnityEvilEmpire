using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 velocityVector;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetVelocity(Vector3 vel)
    {
        velocityVector = vel;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocityVector * moveSpeed;
    }
}
