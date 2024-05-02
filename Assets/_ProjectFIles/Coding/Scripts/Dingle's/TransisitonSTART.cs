using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TransisitonSTART : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutTimeline;
    private double cutsceneDuration;
    // Start is called before the first frame update
    void Awake()
    {
        cutTimeline = cutTimeline.GetComponent<PlayableDirector>();
        cutsceneDuration = cutTimeline.GetComponent<PlayableDirector>().duration;
    }

    // Update is called once per frame
    void Update()
    {

        if (cutTimeline.time == cutsceneDuration)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //LoadGameHere
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cutTimeline.Play();
        }
    }
}


