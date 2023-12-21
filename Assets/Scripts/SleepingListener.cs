using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingListener : MonoBehaviour
{
    [SerializeField]
    private float alertTime = 7;

    [SerializeField]
    private AudioClip snoreLoop;
    [SerializeField]
    private AudioClip alertSound;
    [SerializeField]
    private AudioClip awakeSound;


    private float timer = 0;
    private AudioSource audioSource;


    private SleepState sleepState = SleepState.Heavy;

    private enum SleepState
    {
        Heavy,
        Alert
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sleepState == SleepState.Alert)
        {
            if (timer < alertTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                sleepState = SleepState.Heavy;
                audioSource.loop = true;
                audioSource.clip = snoreLoop;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    public void LoudSound()
    {
        if (sleepState == SleepState.Heavy)
        {
            sleepState = SleepState.Alert;
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.PlayOneShot(alertSound);
            timer = 0;
        }
        else if (sleepState == SleepState.Alert)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.PlayOneShot(awakeSound);
            GameManager.instance.LoseGame();
        }
    }
}
