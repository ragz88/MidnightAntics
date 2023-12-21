using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerChangeTrigger : MonoBehaviour
{
    [SerializeField]
    private SleepingListener targetListener;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.currentListener = targetListener;
        }
    }
}
