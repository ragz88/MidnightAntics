using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpObject : MonoBehaviour
{
    public MovementType movementType = MovementType.Standard;

    public Vector3 leftHoldOffsetPosition;
    public Vector3 rightHoldOffsetPosition;
    public Quaternion leftHoldOffsetRotation;
    public Quaternion rightHoldOffsetRotation;


    [Header("Drawer Settings")]
    public float maxDrawerDistance = 1f;


    [HideInInspector]
    public Vector3 initPosition;

    /// <summary>
    /// Different objects move in different ways, with various limitations. Draws slide on one axis. Doors swing.
    /// </summary>
    public enum MovementType
    {
        Standard,
        Drawer,
        Door
    }

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
