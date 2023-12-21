using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarController : MonoBehaviour
{
    public CondimentType contents;
    
    [HideInInspector]
    public bool isOpen = false;


    private PickUpObject jamPickUp;
    private GameObject realLid;


    [SerializeField]
    private PickUpObject lidPickUp;
    [SerializeField]
    private GameObject fakeLid;
    [SerializeField]
    private Collider jamTrigger;



    // Start is called before the first frame update
    void Start()
    {
        realLid = lidPickUp.gameObject;
        jamPickUp = GetComponent<PickUpObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            if (jamPickUp.isHeld)
            {
                realLid.SetActive(true);
                fakeLid.SetActive(false);
            }
            else
            {
                realLid.SetActive(false);
                fakeLid.SetActive(true);
            }

            if (lidPickUp.isHeld)
            {
                isOpen = true;
                realLid.transform.parent = null;
                realLid.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
        else
        {
            jamTrigger.enabled = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if open and trigger ting is a knife.
        if (other.CompareTag("Knife") && isOpen)
        {
            KnifeController knifeController = other.GetComponent<KnifeController>();
            knifeController.AddCondiment(contents);
        }
    }
}
