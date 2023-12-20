using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandObjectHolder : MonoBehaviour
{
    [SerializeField] float sphereCastRadius = 0.2f;
    [SerializeField] float pickUpRange = 1;
    [SerializeField] LayerMask castLayerMask;

    [SerializeField] HandType handSide;


    private const float SPHERE_CAST_DIST = 3f;


    private bool holdingObject = false;
    private float pickUpSpeed = 1f;
    private Collider objectCollider;
    private Transform objectTrans;
    private PickUpObject pickUpObject;
    private Rigidbody pickUpBody;


    [System.Serializable]
    private enum HandType
    {
        Left,
        Right
    }


    public float PickUpSpeed
    {
        set { pickUpSpeed = value; }
        get { return pickUpSpeed; }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectTrans != null && pickUpObject != null)
        {
            if (handSide == HandType.Left)
            {
                switch(pickUpObject.movementType)
                {
                    case PickUpObject.MovementType.Standard:
                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.leftHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, pickUpObject.leftHoldOffsetRotation, 0.1f);
                        break;

                    case PickUpObject.MovementType.Drawer:

                        Vector3 potentialPos = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.leftHoldOffsetPosition, pickUpSpeed);
                        potentialPos = objectTrans.InverseTransformPoint(potentialPos);
                        potentialPos = new Vector3(pickUpObject.initPosition.x, pickUpObject.initPosition.y, potentialPos.z);
                        objectTrans.localPosition = potentialPos;

                        /*if (Vector3.Distance(objectTrans.position, (transform.position + pickUpObject.leftHoldOffsetPosition)) > 0.2f)
                        {
                            objectTrans.position = Vector3.Lerp(objectTrans.position,
                                new Vector3(objectTrans.position.x, objectTrans.position.y, (transform.position + pickUpObject.leftHoldOffsetPosition).z), pickUpSpeed);
                            if ()
                            {

                            }
                        }


                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.leftHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, pickUpObject.leftHoldOffsetRotation, 0.1f);*/
                        break;

                    case PickUpObject.MovementType.Door:
                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.leftHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, pickUpObject.leftHoldOffsetRotation, 0.1f);
                        break;
                }
                
                
                
                
            }
            else if (handSide == HandType.Right)
            {
                switch (pickUpObject.movementType)
                {
                    case PickUpObject.MovementType.Standard:
                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.rightHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, pickUpObject.rightHoldOffsetRotation, 0.1f);
                        break;

                    case PickUpObject.MovementType.Drawer:
                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.rightHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, pickUpObject.rightHoldOffsetRotation, 0.1f);
                        break;

                    case PickUpObject.MovementType.Door:
                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.rightHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, pickUpObject.rightHoldOffsetRotation, 0.1f);
                        break;
                }
                
            }
            
        }
    
    }


    /// <summary>
    /// Checks vicinity of the hand for any objects that can be picked up, lerping them to the hand if any are found.
    /// </summary>
    public void PickUp()
    {
        if (!holdingObject)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position + (sphereCastRadius * 4 * Vector3.up), sphereCastRadius, Vector3.down, out hit, sphereCastRadius * 4 * pickUpRange, castLayerMask))
            {
                holdingObject = true;
                objectCollider = hit.collider;
                objectTrans = hit.collider.transform;
                objectCollider.enabled = false;
                pickUpObject = objectTrans.GetComponent<PickUpObject>();
                pickUpBody = objectTrans.GetComponent<Rigidbody>();

                //pickUpBody.freezeRotation = true;
            }
        }
    }


    public void DropObject()
    {
        holdingObject = false;
        objectTrans = null;
        pickUpObject = null;

        if (objectCollider != null)
        {
            objectCollider.enabled = true;
            objectCollider = null;
        }

        if (pickUpBody != null)
        {
            //pickUpBody.freezeRotation = false;
            pickUpBody.velocity = Vector3.zero;
            pickUpBody = null;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereCastRadius);
    }
}
