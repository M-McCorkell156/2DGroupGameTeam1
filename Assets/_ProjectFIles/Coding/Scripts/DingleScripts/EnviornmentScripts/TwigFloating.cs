using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigFloating : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB, pointC;
    [SerializeField] private float twigSpeed;
    [SerializeField] private float waitTime;
    private float speed;

    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        speed = twigSpeed;
        nextPosition = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);
        if (transform.position == pointA.position)
        {
            transform.position = pointC.position;
            StartCoroutine(Waiting(gameObject));


        }
        if (transform.position == pointB.position)
        {
            //trying to make platform wait at Point B for a couple seconds
            StartCoroutine(Waiting(gameObject));
        }

        

    }

    

    



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
        yield return new WaitForSeconds(1.6f);
        speed = 0f;
        Debug.Log("Waiting");
        yield return new WaitForSeconds(waitTime);
        speed = twigSpeed;
        //Not waiting for some reason
        
        //My attempt at getting it to wait
    }

}
