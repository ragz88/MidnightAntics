using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public Transform newPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

        if (otherRigidbody != null)
        {
            otherRigidbody.velocity = Vector3.zero;
        }

        other.transform.position = newPos.position;
    }
}
