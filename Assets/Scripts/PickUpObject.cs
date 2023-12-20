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

    [Space]
    [Header("Drawer Settings")]
    public float maxDrawerDistance = 1f;

    [Space]
    [Header("Door Settings")]
    public float maxDoorDistance = 1f;
    public float closedAngle = 0;
    public float openAngle = 90;
    public Transform doorTrans;
    public Transform handleVisualsPos;


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
        initPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
