using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Manager_Matt : MonoBehaviour
{
    private Canvas canvas;
    public Canvas titleCanvas;

    public CanvasGroup group;

    public GameObject player;

    private bool fadeIn = false;

    private bool fadeOut = false;

    private bool fadeInLoop = false;

    private bool fadeOutLoop = false;

    [SerializeField] private float alphaMultiplier;

    void Start()
    {
        //Debug.Log("Start Pause");
        player = GameObject.Find("Player_Corto");
        canvas = GetComponent<Canvas>();
        group.alpha = 0f;
        fadeIn = true;
    }

    void Update()
    {
        if (!titleCanvas.enabled && Input.GetKey(KeyCode.Escape))
        {

            //Debug.Log("Pause");
            if (fadeIn)
            {
                fadeInLoop = true;
            }
            if (fadeOut)
            {
                fadeOutLoop = true;
            }
        }
        if (fadeInLoop)
        {
            FadeIn();
        }
        if (fadeOutLoop)
        {
            FadeOut();
        }
    }
    private void FadeIn()
    {
        canvas.enabled = true;
        fadeIn = true;
        player.GetComponent<Player_Behaviour>().lockMovement();

        //Debug.Log("Start Fade In");
        if (group.alpha < 1)
        {
            group.alpha += Time.deltaTime * alphaMultiplier;
            //Debug.Log("Fading In");
            if (group.alpha >= 1)
            {
                //Debug.Log("Fading Done");
                fadeIn = false;
                fadeOut = true;
                fadeInLoop = false;
            }
        }

    }
    private void FadeOut()
    {
        fadeOut = true;
        player.GetComponent<Player_Behaviour>().unlockMovement();

        //Debug.Log("Start Fade Out");
        if (group.alpha >= 0)
        {
            group.alpha -= Time.deltaTime * alphaMultiplier;

            if (group.alpha <= 0)
            {
                canvas.enabled = false;
                fadeIn = true;
                fadeOut = false;
                fadeOutLoop = false;
            }
        }
    }

}
