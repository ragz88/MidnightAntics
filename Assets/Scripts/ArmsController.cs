using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsController : MonoBehaviour
{
    
    
    
    [SerializeField] float maxForwardDistance = 1f;
    [SerializeField] float maxReverseDistance = 1f;
    [SerializeField] float maxLeftDistance = 1f;
    [SerializeField] float maxRightDistance = 1f;


    


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



    private bool armsActive = false;
    private bool armsLerping = false;
    private Transform currentLerpPosLeft;
    private Transform currentLerpPosRight;


    // Start is called before the first frame update
    void Start()
    {
        
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
