using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNoise : MonoBehaviour
{

    public float softFallMinVelocity = 1f;
    public float hardFallMinVelocity = 1f;

    public AudioClip softFallNoise;
    public AudioClip hardFallNoise;

    public bool beingHeld = false;

    public GameManager gameManager;


    private AudioSource noiseSource;
    private Rigidbody body;
    private SleepingListener listener;

    private float loudSoundTimer = 0;

    private const float LOUD_SOUND_DELAY = 2;


    // Start is called before the first frame update
    void Start()
    {
        noiseSource = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (loudSoundTimer < LOUD_SOUND_DELAY)
        {
            loudSoundTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface") && !beingHeld)
        {
            if (collision.relativeVelocity.magnitude > hardFallMinVelocity && loudSoundTimer > LOUD_SOUND_DELAY)
            {
                noiseSource.PlayOneShot(hardFallNoise);
                listener = gameManager.currentListener;
                listener.LoudSound();
                loudSoundTimer = 0;
            }
            else if (collision.relativeVelocity.magnitude > softFallMinVelocity)
            {
                noiseSource.PlayOneShot(softFallNoise);
            }
        }
    }
}
