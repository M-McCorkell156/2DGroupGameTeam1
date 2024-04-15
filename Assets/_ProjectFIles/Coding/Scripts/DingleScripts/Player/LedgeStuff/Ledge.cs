using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    #region LedgeStuff
    [Header("Ledge Info")]
    [HideInInspector] public bool ledgeDetected;

    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBegunPos;
    private Vector2 climbOverPos;

    private bool canGrabLedge = true;
    private bool canClimb;

    private Animator animator;
    #endregion
    //------------------------------------------\\
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        CheckForLedge();
        animator.SetBool("CanClimb", canClimb);
    }
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePos = GetComponentInChildren<Ledge_Detection>().transform.position;

            climbBegunPos = ledgePos + offset1;
            climbOverPos = ledgePos + offset2;

            canClimb = true;
        }

        if (canClimb)
        {
            transform.position = climbBegunPos;
            
            //Invoke("LedgeClimbOver", 1f);
        }
    }

    public void LedgeClimbOver()
    {
        Debug.Log("Climbing");
        canClimb = false;
        transform.position = climbOverPos;
        Invoke("AllowLedgeGrab", 1f);
    }
    private void AllowLedgeGrab() => canGrabLedge = true;

    //Something else to do with event animation thing
}
