using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TutorialImage : MonoBehaviour
{
    private Canvas canvas;
    public CanvasGroup group;


    public bool fadeIn = false;

    private bool fadeOut = false;

    private bool fadeInLoop = false;

    private bool fadeOutLoop = false;

    [SerializeField] private float alphaMultiplier;



    public bool isTitle;
    public bool isCutscene;
    [SerializeField] private PlayableDirector transition;
    private double transitionDuration;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = true;
        group.alpha = 0f;

    }

    private void Update()
    {

        if (transition.time == transitionDuration)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.anyKey && !isTitle && isCutscene)
        {
            if (fadeOut)
            {
                fadeOutLoop = true;
            }
        }
        
        if (fadeOutLoop)
        {
            FadeOut();
        }
    }
    public void FadeIn()
    {
        canvas.enabled = true;

        fadeIn = true;

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
                transition.Play();
            }
        }

    }
    private void FadeOut()
    {
        fadeOut = true;

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