using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 4000f;

    public Transform playerBody;
    public bool isIn3D = true;
    public Transform checkpointIn;
    public Transform checkpointOut;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIn3D)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

            SmoothMove(checkpointOut, new Vector3(0, 0, 0));
        }
        else
        {
           SmoothMove(checkpointIn, new Vector3(0, 0, -1));
        }
        
    }
    public void SmoothMove(Transform checkpoint, Vector3 offset)
    {
        Vector3 target = checkpoint.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        if(!isIn3D) transform.LookAt(checkpoint);
    }


}
