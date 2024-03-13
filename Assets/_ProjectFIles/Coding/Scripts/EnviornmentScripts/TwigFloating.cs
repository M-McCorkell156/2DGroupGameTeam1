using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigFloating : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float twigSpeed;
    [SerializeField] private float waitTime;
    private float speed;

    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        speed = twigSpeed;
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
            //trying to make platform wait at Point B for a couple seconds
            //Code not working properly, Logging it prints the message but doesn't actually wait.
            if (transform.position == pointB.position)
            {
                StartCoroutine(Waiting(gameObject));
            }
        }
        

    }

    //Attempt to slow twig down

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("SlowDown"))
    //    {
    //        Debug.Log("AH!");
    //        speed = 1f;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("SlowDown"))
    //    {
    //        speed = 3f;
    //    }
    //}


    //Moves Player with Platform
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
    IEnumerator Waiting(GameObject twig)
    {
        speed = 0f;
        yield return new WaitForSeconds(waitTime);
        speed = twigSpeed;
        //Not waiting for some reason
        Debug.Log("Waiting");
        //My attempt at getting it to wait
    }

}
