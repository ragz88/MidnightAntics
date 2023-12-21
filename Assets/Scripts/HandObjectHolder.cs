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
    public bool HoldingObject
    {
        get { return holdingObject; }
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

                        Vector3 newPos = Vector3.Lerp(objectTrans.localPosition, objectTrans.parent.InverseTransformPoint(transform.position + pickUpObject.leftHoldOffsetPosition), pickUpSpeed);
                        newPos = new Vector3(pickUpObject.initPosition.x, pickUpObject.initPosition.y, newPos.z);

                        if (newPos.z > pickUpObject.maxDrawerDistance)
                        {
                            newPos = new Vector3(newPos.x, newPos.y, pickUpObject.maxDrawerDistance);
                        }
                        else if (newPos.z < 0)
                        {
                            newPos = new Vector3(newPos.x, newPos.y, 0);
                        }

                        objectTrans.localPosition = newPos;


                        break;

                    case PickUpObject.MovementType.Door:
                        
                        Vector3 newHandlePos = Vector3.Lerp(objectTrans.localPosition, objectTrans.parent.InverseTransformPoint(transform.position + pickUpObject.leftHoldOffsetPosition), pickUpSpeed);
                        newHandlePos = new Vector3(pickUpObject.initPosition.x, pickUpObject.initPosition.y, newHandlePos.z);
                        
                        if (newHandlePos.z > pickUpObject.maxDoorDistance)
                        {
                            newHandlePos = new Vector3(newHandlePos.x, newHandlePos.y, pickUpObject.maxDoorDistance);
                        }
                        else if (newHandlePos.z < 0)
                        {
                            newHandlePos = new Vector3(newHandlePos.x, newHandlePos.y, 0);
                        }

                        //objectTrans.localPosition = newHandlePos;
                                                
                        float doorRot = Mathf.Lerp(pickUpObject.closedAngle, pickUpObject.openAngle, (newHandlePos.z/ pickUpObject.maxDoorDistance));
                        pickUpObject.doorTrans.eulerAngles =  new Vector3(0,doorRot,0);
                            
                        objectTrans.position = pickUpObject.handleVisualsPos.position;

                        break;
                }
                
                
                
                
            }
            else if (handSide == HandType.Right)
            {
                switch (pickUpObject.movementType)
                {
                    case PickUpObject.MovementType.Standard:
                        objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.rightHoldOffsetPosition, pickUpSpeed);
                        objectTrans.rotation = Quaternion.Slerp(objectTrans.rotation, 
                            new Quaternion(pickUpObject.rightHoldOffsetRotation.x + transform.rotation.x, pickUpObject.rightHoldOffsetRotation.y + transform.rotation.y,
                            pickUpObject.rightHoldOffsetRotation.z + transform.rotation.z, 0), 0.1f);
                        break;

                    case PickUpObject.MovementType.Drawer:

                        Vector3 newPos = Vector3.Lerp(objectTrans.localPosition, objectTrans.parent.InverseTransformPoint(transform.position + pickUpObject.rightHoldOffsetPosition), pickUpSpeed);
                        newPos = new Vector3(pickUpObject.initPosition.x, pickUpObject.initPosition.y, newPos.z);

                        if (newPos.z > pickUpObject.maxDrawerDistance)
                        {
                            newPos = new Vector3(newPos.x, newPos.y, pickUpObject.maxDrawerDistance);
                        }
                        else if (newPos.z < 0)
                        {
                            newPos = new Vector3(newPos.x, newPos.y, 0);
                        }

                        objectTrans.localPosition = newPos;


                        break;

                    case PickUpObject.MovementType.Door:

                        Vector3 newHandlePos = Vector3.Lerp(objectTrans.localPosition, objectTrans.parent.InverseTransformPoint(transform.position + pickUpObject.rightHoldOffsetPosition), pickUpSpeed);
                        newHandlePos = new Vector3(pickUpObject.initPosition.x, pickUpObject.initPosition.y, newHandlePos.z);

                        if (newHandlePos.z > pickUpObject.maxDoorDistance)
                        {
                            newHandlePos = new Vector3(newHandlePos.x, newHandlePos.y, pickUpObject.maxDoorDistance);
                        }
                        else if (newHandlePos.z < 0)
                        {
                            newHandlePos = new Vector3(newHandlePos.x, newHandlePos.y, 0);
                        }

                        //objectTrans.localPosition = newHandlePos;

                        float doorRot = Mathf.Lerp(pickUpObject.closedAngle, pickUpObject.openAngle, (newHandlePos.z / pickUpObject.maxDoorDistance));
                        pickUpObject.doorTrans.eulerAngles = new Vector3(0, doorRot, 0);

                        objectTrans.position = pickUpObject.handleVisualsPos.position;

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
                
                pickUpObject = objectTrans.GetComponent<PickUpObject>();
                pickUpBody = objectTrans.GetComponent<Rigidbody>();

                if (pickUpObject != null)
                {
                    pickUpObject.isHeld = true;
                    if (!pickUpObject.customColliderHandler)
                    {
                        objectCollider.enabled = false;
                    }
                }

                pickUpBody.freezeRotation = true;
            }
        }
    }


    public void DropObject()
    {
        holdingObject = false;
        if (pickUpObject != null)
        {
            pickUpObject.isHeld = false;
        }

        objectTrans = null;
        

        if (objectCollider != null)
        {
            if (!pickUpObject.customColliderHandler)
            {
                objectCollider.enabled = true;
            }
            objectCollider = null;
        }

        pickUpObject = null;

        if (pickUpBody != null)
        {
            pickUpBody.freezeRotation = false;
            if (!pickUpBody.isKinematic)
            {
                pickUpBody.velocity = Vector3.zero;
            }
            pickUpBody = null;
        }
    }


    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereCastRadius);
    }*/
}
