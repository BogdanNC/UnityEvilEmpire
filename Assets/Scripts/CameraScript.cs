using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    public float movementTime;
    public float rotationAmount;
    public Quaternion newRotation;
    public new GameObject camera;
    //public Transform cameraTransform;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    private CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
        vcam = camera.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }
    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.R) && vcam.m_Lens.FieldOfView > 8 )
        {
            vcam.m_Lens.FieldOfView -= 1; 
        }

        if (Input.GetKey(KeyCode.F) && vcam.m_Lens.FieldOfView < 80)
        {
            vcam.m_Lens.FieldOfView += 1;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 1);
    }


}
