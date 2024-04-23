using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge_Detection : MonoBehaviour
{
    [SerializeField] private float radius;


    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Player_Behaviour player;

    private bool canDetect;

    private void Update()
    {
        if (canDetect == true)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, groundLayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundLayer") && collision.gameObject.tag == "NotClimable")
        {
            //Debug.Log(collision.gameObject.name);
            canDetect = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundLayer") && collision.gameObject.tag == "NotClimable")
        {
            Debug.Log(collision.gameObject.name);
            canDetect = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}