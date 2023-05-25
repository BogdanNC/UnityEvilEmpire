using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float movementTime;
    public float rotationAmount;
    public Quaternion newRotation;
    public Transform cameraTransform;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
        newPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }
    void HandleMovementInput()
    {


        if (Input.GetKey(KeyCode.W))
        {
            newPosition += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            newPosition -= transform.forward;
        }


        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.R) && newZoom.y >= 9)
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F) && newZoom.y <= 65)
        {
            newZoom -= zoomAmount;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, 1);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }


}
