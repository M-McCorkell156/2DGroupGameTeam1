using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sciaphilla : MonoBehaviour
{
    [SerializeField] Collider2D collision;
    private void Awake()
    {
        collision = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level 3");
        }
    }
}