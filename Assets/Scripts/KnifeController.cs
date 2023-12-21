using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{

    [HideInInspector]
    public CondimentType knifeCondiment = CondimentType.None;


    [SerializeField]
    private Collider holdingCollider;
    [SerializeField]
    private Collider standardCollider;

    [SerializeField]
    private GameObject jamBlob;
    [SerializeField]
    private GameObject peanutBlob;
    [SerializeField]
    private GameObject pbjBlob;


    private PickUpObject pickUpObject;


    // Start is called before the first frame update
    void Start()
    {
        pickUpObject = GetComponent<PickUpObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddCondiment(CondimentType condiment)
    {
        if (pickUpObject.isHeld)
        {
            holdingCollider.enabled = true;
            standardCollider.enabled = false;
            if (condiment == CondimentType.PeanutButter)
            {
                if (knifeCondiment == CondimentType.None)
                {
                    knifeCondiment = CondimentType.PeanutButter;
                    peanutBlob.SetActive(true);
                }
                else if (knifeCondiment == CondimentType.Jam)
                {
                    knifeCondiment = CondimentType.PBJ;
                    jamBlob.SetActive(false);
                    pbjBlob.SetActive(true);
                }
            }
            else if (condiment == CondimentType.Jam)
            {
                if (knifeCondiment == CondimentType.None)
                {
                    knifeCondiment = CondimentType.Jam;
                    jamBlob.SetActive(true);
                }
                else if (knifeCondiment == CondimentType.PeanutButter)
                {
                    knifeCondiment = CondimentType.PBJ;
                    peanutBlob.SetActive(false);
                    pbjBlob.SetActive(true);
                }
            }
        }
        else
        {
            holdingCollider.enabled = false;
            standardCollider.enabled = true;
        }
    }
}
