using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ArmsController : MonoBehaviour
{

    [SerializeField] float armSpeed = 1f;


    [SerializeField] float maxForwardDistance = 1f;
    [SerializeField] float maxReverseDistance = 1f;
    [SerializeField] float maxOuterDistance = 1f;
    [SerializeField] float maxInnerDistance = 1f;

    [SerializeField] float armLimitSmoothingStrength = 8f;



    [Header("Arm Objects")]
    [SerializeField] Transform leftArm;
    [SerializeField] Transform rightArm;
    [SerializeField] HandObjectHolder leftObjectHolder;
    [SerializeField] HandObjectHolder rightObjectHolder;


    [Space]
    [Header("Arm Animations")]
    [SerializeField] Animator leftAnim;
    [SerializeField] Animator rightAnim;


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
    [Tooltip("How quickly objects are lerped into the hands.")]
    [SerializeField] float pickUpSpeed = 0.1f;


    private Vector3 leftArmInitPosition;
    private Vector3 rightArmInitPosition;
    private bool armsActive = false;
    private bool armsLerping = false;
    private bool leftHandClosed = false;
    private bool rightHandClosed = false;
    private Transform currentLerpPosLeft;
    private Transform currentLerpPosRight;


    // Start is called before the first frame update
    void Start()
    {

        leftArmInitPosition = initialArmsPositionLeft.localPosition;
        rightArmInitPosition = initialArmsPositionLeft.localPosition;
        leftObjectHolder.PickUpSpeed = pickUpSpeed;
        rightObjectHolder.PickUpSpeed = pickUpSpeed;
        //leftArmInitPosition = leftArm.localPosition;
        //rightArmInitPosition = rightArm.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // When arms are moving to their initial or rest positions
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
        else if (armsActive)
        {
            float leftHorizontalMovement = Input.GetAxis("Horizontal");
            float leftVerticalMovement   = Input.GetAxis("Vertical");
            float rightHorizontalMovement  = Input.GetAxis("RightHandX");
            float rightVerticalMovement  = Input.GetAxis("RightHandY");

            MoveArm(leftArm, leftHorizontalMovement, leftVerticalMovement);
            MoveArm(rightArm, rightHorizontalMovement, rightVerticalMovement);
        }

        // Handles Holding Objects
        float leftGripInput  = Input.GetAxis("LeftGrip");
        float rightGripInput = Input.GetAxis("RightGrip");
        
        if (leftGripInput != 0)
        {
            CloseHand(leftArm);
        }
        else
        {
            OpenHand(leftArm);
        }

        if (rightGripInput != 0)
        {
            CloseHand(rightArm);
        }
        else
        {
            OpenHand(rightArm);
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
        if (arm == leftArm)
        {
            if (horizontalMovement < 0)
            {
                
                // In the nono zone
                if (arm.localPosition.x < initialArmsPositionLeft.localPosition.x - maxOuterDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition, 
                        new Vector3(initialArmsPositionLeft.localPosition.x - maxOuterDistance, arm.localPosition.y, arm.localPosition.z),
                        armLimitSmoothingStrength * Time.deltaTime);
                }
            
            }
            else if (horizontalMovement > 0)
            {
                if (arm.localPosition.x > initialArmsPositionLeft.localPosition.x + maxInnerDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(initialArmsPositionLeft.localPosition.x + maxInnerDistance, arm.localPosition.y, arm.localPosition.z),
                        armLimitSmoothingStrength * Time.deltaTime);
                }
            }


            if (verticalMovement < 0)
            {

                // In the nono zone
                if (arm.localPosition.z < initialArmsPositionLeft.localPosition.z - maxReverseDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(arm.localPosition.x, arm.localPosition.y, initialArmsPositionLeft.localPosition.z - maxReverseDistance),
                        armLimitSmoothingStrength * Time.deltaTime);
                }

            }
            else if (verticalMovement > 0)
            {
                if (arm.localPosition.z > initialArmsPositionLeft.localPosition.z + maxForwardDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(arm.localPosition.x, arm.localPosition.y, initialArmsPositionLeft.localPosition.z + maxForwardDistance),
                        armLimitSmoothingStrength * Time.deltaTime);
                }
            }

            
        }
        else //right arm
        {
            if (horizontalMovement < 0)
            {

                // In the nono zone
                if (arm.localPosition.x < initialArmsPositionRight.localPosition.x - maxInnerDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(initialArmsPositionRight.localPosition.x - maxInnerDistance, arm.localPosition.y, arm.localPosition.z),
                        armLimitSmoothingStrength * Time.deltaTime);
                }

            }
            else if (horizontalMovement > 0)
            {
                if (arm.localPosition.x > initialArmsPositionRight.localPosition.x + maxOuterDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(initialArmsPositionRight.localPosition.x + maxOuterDistance, arm.localPosition.y, arm.localPosition.z),
                        armLimitSmoothingStrength * Time.deltaTime);
                }
            }


            if (verticalMovement < 0)
            {

                // In the nono zone
                if (arm.localPosition.z < initialArmsPositionRight.localPosition.z - maxReverseDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(arm.localPosition.x, arm.localPosition.y, initialArmsPositionRight.localPosition.z - maxReverseDistance),
                        armLimitSmoothingStrength * Time.deltaTime);
                }

            }
            else if (verticalMovement > 0)
            {
                if (arm.localPosition.z > initialArmsPositionRight.localPosition.z + maxForwardDistance)
                {
                    arm.localPosition = Vector3.Lerp(arm.localPosition,
                        new Vector3(arm.localPosition.x, arm.localPosition.y, initialArmsPositionRight.localPosition.z + maxForwardDistance),
                        armLimitSmoothingStrength * Time.deltaTime);
                }
            }
        }

        arm.localPosition = arm.localPosition + new Vector3(horizontalMovement, 0, verticalMovement) * armSpeed * Time.deltaTime;
    }


    
    /// <summary>
    /// Closes the hand (if it's still open), then keeps it that way.
    /// </summary>
    /// <param name="arm">The arm who's hand must be closed.</param>
    void CloseHand(Transform arm)
    {
        if (arm == leftArm)
        {
            leftAnim.SetBool("HandClosed", true);
            leftObjectHolder.PickUp();
        }
        else if (arm == rightArm)
        {
            rightAnim.SetBool("HandClosed", true);
            rightObjectHolder.PickUp();
        }
    }


    /// <summary>
    /// Closes the hand (if it's still open), then keeps it that way.
    /// </summary>
    /// <param name="arm">The arm who's hand must be closed.</param>
    void OpenHand(Transform arm)
    {
        if (arm == leftArm)
        {
            leftAnim.SetBool("HandClosed", false);
            leftObjectHolder.DropObject();
        }
        else if (arm == rightArm)
        {
            rightAnim.SetBool("HandClosed", false);
            rightObjectHolder.DropObject();
        }
    }


    /// <summary>
    /// Puts arms into the correct rest position
    /// </summary>
    public void ArmsRest()
    {
        if (leftObjectHolder.HoldingObject)
        {
            currentLerpPosLeft = holdingRestPositionLeft;
        }
        else
        {
            currentLerpPosLeft = standardRestPositionLeft;
        }

        if (rightObjectHolder.HoldingObject)
        {
            currentLerpPosRight = holdingRestPositionRight;
        }
        else
        {
            currentLerpPosRight = standardRestPositionRight;
        }


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
