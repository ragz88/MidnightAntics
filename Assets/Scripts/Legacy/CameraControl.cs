using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraControl : MonoBehaviour
{

    [SerializeField] float lookSensitivity;

    public Transform playerBody;

    float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("LookX") * lookSensitivity * Time.deltaTime;
        float yInput = Input.GetAxis("LookY") * lookSensitivity * Time.deltaTime;

        xRotation -= yInput;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * xInput);
    }
}
