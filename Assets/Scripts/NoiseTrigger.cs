using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTrigger : MonoBehaviour
{

    private AudioSource noiseSource;

    private SleepingListener listener;

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
        if (other.CompareTag("Player") && !noiseSource.isPlaying)
        {
            FirstPersonController playerController = GetComponent<FirstPersonController>();

            if (playerController != null)
            {
                if (!playerController.isCrouched)
                {
                    noiseSource.Play();
                    listener = GameManager.instance.currentListener;
                    listener.LoudSound();
                }
            }

        }
    }
}
