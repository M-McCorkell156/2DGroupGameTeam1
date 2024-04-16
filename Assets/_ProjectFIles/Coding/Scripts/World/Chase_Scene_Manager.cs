using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Scene_Manager : MonoBehaviour
{
    private bool _isChasing; 

    [SerializeField] private Transform _targetPos;
    public Rigidbody2D RB { get; private set; }
    public void BeginChase()
    {
        _isChasing = true;
    }

    private void FixedUpdate()
    {
        if (_isChasing)
        {
            //RB.AddForce
        }
    }
}
