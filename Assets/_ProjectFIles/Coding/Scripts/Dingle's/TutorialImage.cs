using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TutorialImage : MonoBehaviour
{
    [SerializeField] private PlayableDirector transition;
    private double transitionDuration;
    private bool isTransition;

    private void Awake()
    {
        
        transitionDuration = transition.duration;
    }

    private void Update()
    {
        if (isTransition)
        {
            if (transition.time == transitionDuration )
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transition.Play();
    }


}