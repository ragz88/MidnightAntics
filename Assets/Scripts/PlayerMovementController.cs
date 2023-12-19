using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    
    NavMeshAgent agent;

    Vector3 initialDestinationPos;

    public Transform destinationTrans;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");                     // Upgrade this to stop smoothly
        float horizontal = Input.GetAxisRaw("Horizontal");


        destinationTrans.position = transform.position + new Vector3(horizontal, 0, vertical).normalized;

        agent.SetDestination(destinationTrans.position);
    }
}
