using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCameraScript : MonoBehaviour
{
    //public Transform camera;
    float speed = 20f;
    public Quaternion newRotation;

    void Start()
    {
        newRotation = transform.rotation;
        
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
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 vec = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.position += vec * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.position -= transform.forward * Time.deltaTime * speed;
            Vector3 vec = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.position -= vec * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 vector = new Vector3(0, 1f, 0);
            //newRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * -rotationAmount);
            transform.RotateAround(transform.position, -Vector3.up, 20 * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 vector = new Vector3(0, 1f, 0);
            //newRotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * rotationAmount);
            transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
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

        //transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 1);
    }

}
