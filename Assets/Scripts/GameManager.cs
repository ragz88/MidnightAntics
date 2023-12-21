using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public FirstPersonController playerController;
    public AudioClip stepSound1;
    public AudioClip stepSound2;

    public Image gameEndPanel;
    public Image gameOver;
    public Image winScreen;

    public static GameManager instance;

    public SleepingListener currentListener;


    bool gameEnded = false;
    float panelInitAlpha;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        playerController.stepSound1 = stepSound1;
        playerController.stepSound2 = stepSound2;
    }


    // Start is called before the first frame update
    void Start()
    {
        panelInitAlpha = gameEndPanel.color.a;
        gameEndPanel.color = new Color(gameEndPanel.color.r, gameEndPanel.color.b, gameEndPanel.color.g, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (gameEnded)
        {
            gameEndPanel.color = Color.Lerp(gameEndPanel.color, gameEndPanel.color + new Color(0,0,0,panelInitAlpha), 1 * Time.deltaTime);
        }
    }


    public void WinGame()
    {
        gameEnded = true;
        winScreen.gameObject.SetActive(true);
        gameEndPanel.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        gameEnded = true;
        gameOver.gameObject.SetActive(true);
        gameEndPanel.gameObject.SetActive(true);
    }
}
