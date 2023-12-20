using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic that switches between walking controls and Arm Controls
/// </summary>
public class ControlsManager : MonoBehaviour
{
    FirstPersonController walkingController;
    ArmsController armsController;


    private bool isWalking = true;

    private void Start()
    {
        walkingController = GetComponent<FirstPersonController>();
        armsController = GetComponent<ArmsController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ControlSwitch"))
        {
            if (isWalking)
            {
                isWalking = false;
                walkingController.walkingMode = false;
                armsController.ArmsLift();
            }
            else
            {
                isWalking = true;
                walkingController.walkingMode = true;
                armsController.ArmsRest();
            }
        }
    }
}
