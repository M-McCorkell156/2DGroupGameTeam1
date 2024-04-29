using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDisappear : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Disappear");
            spriteRenderer.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteRenderer.enabled = true;
        }
    }
}
