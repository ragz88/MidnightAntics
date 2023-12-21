using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadController : MonoBehaviour
{
    [HideInInspector]
    public CondimentType breadCondiment = CondimentType.None;


    [SerializeField]
    private GameObject jamBlob;
    [SerializeField]
    private GameObject peanutBlob;
    [SerializeField]
    private GameObject pbjBlob;


    [SerializeField]
    private Collider standardCollider;

    [SerializeField]
    private HandObjectHolder leftHolder;
    [SerializeField]
    private HandObjectHolder rightHolder;
    [SerializeField]
    private GameObject sandwich;

    private PickUpObject pickUpObject;

    // Start is called before the first frame update
    void Start()
    {
        pickUpObject = GetComponent<PickUpObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUpObject.isHeld)
        {
            standardCollider.enabled = false;
        }
        else
        {
            standardCollider.enabled = true;
        }
    }

    public void AddCondiment(CondimentType condiment)
    {

        if (condiment == CondimentType.PeanutButter)
        {
            if (breadCondiment == CondimentType.None)
            {
                breadCondiment = CondimentType.PeanutButter;
                peanutBlob.SetActive(true);
            }
            else if (breadCondiment == CondimentType.Jam)
            {
                breadCondiment = CondimentType.PBJ;
                jamBlob.SetActive(false);
                pbjBlob.SetActive(true);
            }
        }
        else if (condiment == CondimentType.Jam)
        {
            if (breadCondiment == CondimentType.None)
            {
                breadCondiment = CondimentType.Jam;
                jamBlob.SetActive(true);
            }
            else if (breadCondiment == CondimentType.PeanutButter)
            {
                breadCondiment = CondimentType.PBJ;
                peanutBlob.SetActive(false);
                pbjBlob.SetActive(true);
            }
        }
        else if (condiment == CondimentType.PBJ)
        {
            if (breadCondiment == CondimentType.None)
            {
                breadCondiment = CondimentType.PBJ;
                pbjBlob.SetActive(true);
            }
            else if (breadCondiment == CondimentType.PeanutButter)
            {
                breadCondiment = CondimentType.PBJ;
                peanutBlob.SetActive(false);
                pbjBlob.SetActive(true);
            }
            else if (breadCondiment == CondimentType.Jam)
            {
                breadCondiment = CondimentType.PBJ;
                jamBlob.SetActive(false);
                pbjBlob.SetActive(true);
            }
        }
    }



    private void MakeSandwich(GameObject otherSlice)
    {
        leftHolder.DropObject();
        rightHolder.DropObject();
        otherSlice.SetActive(false);
        Instantiate(sandwich, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if open and trigger ting is a knife.
        if (other.CompareTag("Knife"))
        {
            KnifeController knifeController = other.GetComponent<KnifeController>();
            AddCondiment(knifeController.knifeCondiment);
        }
        else if (other.CompareTag("Bread"))
        {
            BreadController otherSliceController = other.GetComponent<BreadController>();
            if (breadCondiment == CondimentType.PBJ ||
                otherSliceController.breadCondiment == CondimentType.PBJ ||
                (breadCondiment == CondimentType.PeanutButter && otherSliceController.breadCondiment == CondimentType.Jam) ||
                (breadCondiment == CondimentType.Jam && otherSliceController.breadCondiment == CondimentType.PeanutButter))
            {
                MakeSandwich(other.gameObject);
            }
        }
    }
}
