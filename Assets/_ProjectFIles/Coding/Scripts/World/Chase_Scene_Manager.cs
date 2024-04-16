using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Scene_Manager : MonoBehaviour
{
    private bool _isChasing; 

    public GameObject[] target;
    public GameObject endPoint;
    [Space(5)]
    private int current;
    [SerializeField] private float speed;
    public Rigidbody2D RB { get; private set; }
    public void BeginChase()
    {
        _isChasing = true;
        current = 0;
        //Debug.Log("Begin Chase");
    }

    private void FixedUpdate()
    {
        if (_isChasing)
        {
            if (Vector3.Distance(transform.position, endPoint.transform.position) < 1)
            {
                _isChasing = false;
            }
            else if (Vector3.Distance(target[current].transform.position, transform.position) < 1)
            {
                current ++;
                if (current >= target.Length)
                {
                    current = 0;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, target[current].transform.position, Time.deltaTime * speed);
            //Debug.Log("After Move");
        }
    }
}
