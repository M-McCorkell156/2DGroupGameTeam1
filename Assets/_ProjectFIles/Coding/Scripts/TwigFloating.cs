using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigFloating : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float speed;

    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);
        if (transform.position == nextPosition)
        {
            Debug.Log(nextPosition.ToString());
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
            if (transform.position == pointB.position)
            {
                StartCoroutine(Wait(gameObject));
            }
        }
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlowDown"))
        {
            Debug.Log("AH!");
            speed = 1f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlowDown"))
        {
            speed = 3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }

    }
    IEnumerator Wait(GameObject twig)
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Waiting");
        transform.position = transform.position;
    }

}
