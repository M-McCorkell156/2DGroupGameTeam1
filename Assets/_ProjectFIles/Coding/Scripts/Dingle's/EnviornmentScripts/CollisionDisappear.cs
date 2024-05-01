using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CollisionDisappear : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private float red;
    private float green;
    private float blue;
    private float alpha;
    [SerializeField] private float alphaMultiplier;
    private bool fadeIn = false;

    private bool fadeOut = false;

    private bool fadeInLoop = false;

    private bool fadeOutLoop = false;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        red = GetComponent<SpriteRenderer>().color.r;
        green = GetComponent<SpriteRenderer>().color.g;
        blue = GetComponent<SpriteRenderer>().color.b;
        alpha = GetComponent<SpriteRenderer>().color.a;
        fadeIn = true;
    }
    private void Update()
    {
        spriteRenderer.color = new Color(red, green, blue, alpha);
        if (fadeIn)
        {
            fadeInLoop = true;
        }
        if (fadeOut)
        {
            fadeOutLoop = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Disappear");
            fadeOut = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fadeIn = true;
        }
    }
    private void FadeIn()
    {
        fadeIn = true;
        if (alpha < 1)
        {
            alpha += Time.deltaTime * alphaMultiplier;
            //Debug.Log("Fading In");
            if (alpha >= 1)
            {
                //Debug.Log("Fading Done");
                fadeIn = false;
                
                fadeInLoop = false;
            }
        }
    }

    private void FadeOut()
    {
        fadeOut = true;
        
        //Debug.Log("Start Fade Out");
        if (alpha >= 0)
        {
            alpha -= Time.deltaTime * alphaMultiplier;

            if (alpha <= 0)
            {
               
                
                fadeOut = false;
                fadeOutLoop = false;
            }
        }
    }
}
