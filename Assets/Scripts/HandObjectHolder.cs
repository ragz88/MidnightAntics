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


    private float pickUpSpeed = 1f;
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
                objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.leftHoldOffsetTransform.localPosition, pickUpSpeed);
            }
            else
            {
                objectTrans.position = Vector3.Lerp(objectTrans.position, transform.position + pickUpObject.rightHoldOffsetTransform.localPosition, pickUpSpeed);
            }
            
        }
    
    }


    /// <summary>
    /// Checks vicinity of the hand for any objects that can be picked up, lerping them to the hand if any are found.
    /// </summary>
    public void PickUp()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + (sphereCastRadius * 4 * Vector3.up), sphereCastRadius, Vector3.down, out hit, sphereCastRadius * 4 * pickUpRange, castLayerMask))
        {
            objectTrans = hit.collider.transform;
            pickUpObject = objectTrans.GetComponent<PickUpObject>();
            pickUpBody = objectTrans.GetComponent<Rigidbody>();

            pickUpBody.freezeRotation = true;
        }
    }


    public void DropObject()
    {
        objectTrans = null;
        pickUpObject = null;

        if (pickUpBody != null)
        {
            pickUpBody.freezeRotation = false;
            pickUpBody = null;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereCastRadius);
    }
}
