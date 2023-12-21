using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutFadeTrigger : MonoBehaviour
{

    public Image[] tutorials;
    public float fadeSpeed = 0.2f;

    bool isFading = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            for (int i = 0; i < tutorials.Length; i ++)
            {
                tutorials[i].color = tutorials[i].color - new Color(0,0,0, fadeSpeed*Time.deltaTime);
            }

            if (tutorials[0].color.a < 0.05f)
            {
                for (int i = 0; i < tutorials.Length; i++)
                {
                    tutorials[i].gameObject.SetActive(false);
                }

                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFading = true;
        }
    }
}
