using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutscene : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private PlayableDirector titleTimeline;
    [SerializeField] private PlayableDirector cutsceneTimeline;
    private double titleDuration;
    private double cutsceneDuration;

    
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
        if (titleTimeline.time == titleDuration)
        {
            Debug.Log("Loading");
            cutsceneTimeline.Play();
            if (cutsceneTimeline.time == cutsceneDuration)
            {
                Debug.Log("LoadingGame!");
                //LoadGameHere
            }

        }
    }
}
