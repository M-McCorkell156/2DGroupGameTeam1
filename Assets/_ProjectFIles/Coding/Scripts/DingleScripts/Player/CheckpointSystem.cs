using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    
    [SerializeField] private Transform respawn;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            respawn.position = transform.position;
            Destroy(collision.gameObject);
        }



        if (collision.gameObject.CompareTag("Death"))
        {
            animator.SetBool("Death", true);
        }

    }
    public void DeathEvent()
    {
        animator.SetBool("Death", false);
        transform.position = respawn.position;
        
    }
}
