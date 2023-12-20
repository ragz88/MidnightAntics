using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Tooltip("The positions and rotations in these transforms will overwrite the default positions when lerping the object into the hand.")]
    public Transform leftHoldOffsetTransform;
    public Transform rightHoldOffsetTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
