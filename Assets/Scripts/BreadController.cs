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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    }



    private void MakeSandwich(GameObject otherSlice)
    {
        Debug.Log("Winner");
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
