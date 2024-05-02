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
        transition = transition.GetComponent<PlayableDirector>();
        transitionDuration = transition.duration;
        Debug.Log(transitionDuration);
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (isTransition)
        {
            Debug.Log(transition.time);
            if (transition.time == transitionDuration )
            {
                Debug.Log("Loading");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTransition = true;
        transition.Play();
    }


}