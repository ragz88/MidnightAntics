using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ArmsController : MonoBehaviour
{

    [SerializeField] float armSpeed = 1f;


    [SerializeField] float maxForwardDistance = 1f;
    [SerializeField] float maxReverseDistance = 1f;
    [SerializeField] float maxSideDistance = 1f;


    


    [Header("Arm Objects")]
    [SerializeField] Transform leftArm;
    [SerializeField] Transform rightArm;

    [Space]
    [Header("Arm Lerping")]
    [Tooltip("Where hands will normally be while walking.")]
    [SerializeField] Transform standardRestPositionLeft;
    [Tooltip("Where hands will normally be while walking.")]
    [SerializeField] Transform standardRestPositionRight;
    [Tooltip("Hand position while walking, if the hand is holding something.")]
    [SerializeField] Transform holdingRestPositionLeft;
    [Tooltip("Hand position while walking, if the hand is holding something.")]
    [SerializeField] Transform holdingRestPositionRight;
    [Tooltip("Point hands initially lerp to when entering arm mode.")]
    [SerializeField] Transform initialArmsPositionLeft;
    [Tooltip("Point hands initially lerp to when entering arm mode.")]
    [SerializeField] Transform initialArmsPositionRight;

    [Tooltip("How fast the arms move to their key positions")]
    [SerializeField] float lerpSpeed = 2f;
    [Tooltip("How close the hand needs to be to the final position before the lerp will end.")]
    [SerializeField] float lerpAccuracy = 0.1f;


    private Vector3 leftArmInitPosition;
    private Vector3 rightArmInitPosition;
    private bool armsActive = false;
    private bool armsLerping = false;
    private Transform currentLerpPosLeft;
    private Transform currentLerpPosRight;


    // Start is called before the first frame update
    void Start()
    {
        leftArmInitPosition = leftArm.localPosition;
        rightArmInitPosition = rightArm.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (armsLerping)
        {
            leftArm.position = Vector3.Lerp(leftArm.position, currentLerpPosLeft.position, lerpSpeed * Time.deltaTime);
            leftArm.rotation = Quaternion.Slerp(leftArm.rotation, currentLerpPosLeft.rotation, lerpSpeed * Time.deltaTime);

            rightArm.position = Vector3.Lerp(rightArm.position, currentLerpPosRight.position, lerpSpeed * Time.deltaTime);
            rightArm.rotation = Quaternion.Slerp(rightArm.rotation, currentLerpPosRight.rotation, lerpSpeed * Time.deltaTime);

            // Check if lerp is complete
            if (Vector3.Distance(leftArm.position, currentLerpPosLeft.position) < lerpAccuracy &&
                Vector3.Distance(rightArm.position, currentLerpPosRight.position) < lerpAccuracy)
            {
                armsLerping = false;
            }
        }


        // Main arms movement controls should only function when in Arms movement mode
        if (armsActive)
        {
            float leftHorizontalMovement = Input.GetAxis("Horizontal");
            float leftVerticalMovement   = Input.GetAxis("Vertical");
            float rightHorizontalMovement  = Input.GetAxis("LookX");
            float rightVerticalMovement  = Input.GetAxis("LookY");

            MoveArm(leftArm, leftHorizontalMovement, leftVerticalMovement);
            MoveArm(rightArm, rightHorizontalMovement, rightVerticalMovement);

        }
    }


    /// <summary>
    /// Moves the specified arm based on the player input. Applies relevant clamps and speed adjustments.
    /// </summary>
    /// <param name="arm">Arm to be moved.</param>
    /// <param name="horizontalMovement">Float representing the horizontal movement requested by the player.</param>
    /// <param name="verticalMovement">Float representing the vertical movement requested by the player.</param>
    void MoveArm(Transform arm, float horizontalMovement, float verticalMovement)
    {
        if (arm = leftArm)
        {
            horizontalMovement = Mathf.Clamp(horizontalMovement, leftArmInitPosition.x - maxSideDistance, leftArmInitPosition.x + maxSideDistance);
            verticalMovement = Mathf.Clamp(verticalMovement, leftArmInitPosition.z - maxReverseDistance, leftArmInitPosition.x + maxForwardDistance);
        }
        else
        {
            horizontalMovement = Mathf.Clamp(horizontalMovement, rightArmInitPosition.x - maxSideDistance, rightArmInitPosition.x + maxSideDistance);
            verticalMovement = Mathf.Clamp(verticalMovement, rightArmInitPosition.z - maxReverseDistance, rightArmInitPosition.x + maxForwardDistance);
        }

        
    }


    /// <summary>
    /// Puts arms into the correct rest position
    /// </summary>
    public void ArmsRest()
    {
        currentLerpPosLeft = standardRestPositionLeft;
        currentLerpPosRight = standardRestPositionRight;
        armsActive = false;
        armsLerping = true;
    }

    /// <summary>
    /// Puts arms into the active position, ready to act.
    /// </summary>
    public void ArmsLift()
    {
        currentLerpPosLeft = initialArmsPositionLeft;
        currentLerpPosRight = initialArmsPositionRight;
        armsActive = true;
        armsLerping = true;
    }
}
