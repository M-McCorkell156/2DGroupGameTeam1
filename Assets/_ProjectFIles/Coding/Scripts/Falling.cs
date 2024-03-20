using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Falling : MonoBehaviour
{
    private GameObject[] debris;
    

    private void Awake()
    {
        debris = GameObject.FindGameObjectsWithTag("Debris");
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(getBalls(debris));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator getBalls(GameObject[] debris)
    {
        yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < debris.Length; i++)
        {
            debris[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
