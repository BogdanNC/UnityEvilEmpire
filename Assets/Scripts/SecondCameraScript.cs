using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCameraScript : MonoBehaviour
{
    //public Transform camera;
    float speed = 20f;
    public Quaternion newRotation;

    public Vector2 mouseRotation;
    public float sensibility = 2f;

    void Start()
    {

    }

    void Update()
    {
        if (GetComponent<Camera>().enabled == true)
        {

            HandleInput();
        }
    }
    void HandleInput()
    {

        mouseRotation.x += Input.GetAxis("Mouse X");
        mouseRotation.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-mouseRotation.y * sensibility, mouseRotation.x * sensibility, 0);

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 vec = new Vector3(transform.forward.x * 2f, transform.forward.y * 2f, transform.forward.z * 2f);
            transform.position += vec * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.position -= transform.forward * Time.deltaTime * speed;
            Vector3 vec = new Vector3(transform.forward.x * 2f, transform.forward.y * 2f, transform.forward.z * 2f);
            transform.position -= vec * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 vector = new Vector3(transform.right.x * 2f, 0, transform.right.z * 2f);
            transform.position -= vector * Time.deltaTime * speed;
            //newRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * -rotationAmount);
            //transform.RotateAround(transform.position, -Vector3.up, 20 * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 vector = new Vector3(transform.right.x * 2f, 0, transform.right.z * 2f);
            transform.position += vector * Time.deltaTime * speed;
            //newRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * rotationAmount);
            //transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
            
        }


        if (Input.GetKey(KeyCode.E))
        {
            Vector3 vector = new Vector3(0, 2f, 0);
            transform.position += vector * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 vector = new Vector3(0, 2f, 0);
            transform.position -= vector * Time.deltaTime * speed;
        }

    }

}
