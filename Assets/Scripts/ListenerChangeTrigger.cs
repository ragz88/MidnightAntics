using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerChangeTrigger : MonoBehaviour
{
    [SerializeField]
    private SleepingListener targetListener;

    public GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.currentListener = targetListener;
        }
    }
}
