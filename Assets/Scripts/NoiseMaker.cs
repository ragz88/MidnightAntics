using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    public SleepingListener listener;
    
    // Start is called before the first frame update
    void Start()
    {
        listener = GameManager.instance.currentListener;
        listener.LoudSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
