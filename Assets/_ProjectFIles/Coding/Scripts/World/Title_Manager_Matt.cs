using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class World_Manager_Matt : MonoBehaviour
{
    #region Variables

    private Canvas canvas;
    public Canvas pauseCanvas;

    public CanvasGroup group;

    public GameObject player;

    [SerializeField] private bool fadeIn = false;

    [SerializeField] private bool fadeOut = false;

    private bool activteScreen;

    public float spawnTime;

    #endregion
    void Start()
    {
        pauseCanvas.enabled = false;
        player = GameObject.Find("Player_Corto");
        canvas = GetComponent<Canvas>();
        canvas.enabled = true;
        group.alpha = 0f;
        fadeIn = true;
        FadeIn();
    }

    void Update()
    {
        if (fadeIn)
        {
            FadeIn();
        }
        if (Input.anyKey && activteScreen)
        {
            FadeOut();
        }
        if (fadeOut)
        {
            FadeOut();
        }

    }
    private void FadeIn()
    {
        //Debug.Log("Start Fade In");

        if (group.alpha < 1)
        {
            group.alpha += Time.deltaTime;
            //Debug.Log("Fading In");


            if (group.alpha >= 1)
            {
                //Debug.Log("Fading Done");
                canvas.enabled = true;
                fadeIn = false;
                activteScreen = true;

            }
        }

    }
    private void FadeOut()
    {
        activteScreen = false;
        fadeOut = true;
        //Debug.Log("Start Fade Out");
        if (group.alpha >= 0)
        {
            group.alpha -= Time.deltaTime;

            if (group.alpha <= 0)
            {
                fadeOut = false;
                StartCoroutine(ActiavetePlayer());
            }
        }
    }

    IEnumerator ActiavetePlayer()
    {
        yield return new WaitForSeconds(spawnTime);
        //Debug.Log("Wait Done");
        canvas.enabled = false;
        pauseCanvas.enabled = true;
        player.GetComponent<Player_Behaviour>().unlockMovement();
    }
}
