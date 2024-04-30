using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class StartCutscene : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private PlayableDirector titleTimeline;
    [SerializeField] private PlayableDirector cutsceneTimeline;
    private double titleDuration;
    private double cutsceneDuration;
    private bool isCutscene;
    private bool isTitle;

    
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = true;
        titleTimeline = titleTimeline.GetComponent<PlayableDirector>();
        titleDuration = titleTimeline.GetComponent<PlayableDirector>().duration;
        cutsceneTimeline = cutsceneTimeline.GetComponent<PlayableDirector>();
        cutsceneDuration = cutsceneTimeline.GetComponent <PlayableDirector>().duration;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !isTitle && !isCutscene)
        {
            titleTimeline.Play();
            isTitle = true;
        }
        if (titleTimeline.time == titleDuration)
        {
            Debug.Log("Loading");
            cutsceneTimeline.Play();
            isCutscene = true;
            

        }
        if (cutsceneTimeline.time == cutsceneDuration)
        {
            Debug.Log("LoadingGame!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //LoadGameHere
        }
    }
}
