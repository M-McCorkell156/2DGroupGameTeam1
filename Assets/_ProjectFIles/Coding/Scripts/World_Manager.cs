using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    private Canvas canvas;

    public GameObject player;
    public GameObject badGuy;

    public AudioSource background;
    public AudioSource source;

    private void Start()
    {
        UnityEngine.UI.Button restart = GameObject.Find("RestartButton").GetComponent<UnityEngine.UI.Button>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        restart.onClick.AddListener(Reset);
    }
    public void GameOver()
    {
        background.Pause();
        source.Play();
        Destroy(player);
        Destroy(badGuy);
        canvas.enabled = true;

    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
    }
}
