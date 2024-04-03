using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    public Player_Behaviour_Data data;

    #region Components 
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public bool IsFacingRight { get; private set; }
    public bool isJumping { get; private set; }
    public bool IsChuting { get; private set; }
    public bool IsSticking { get; private set; }

    //Timers
    public float LastOnGroundTime { get; private set; }

    //Jump
    private bool isJumpCut;
    private bool isJumpfalling;
    #endregion

    #region Inputs
    private Vector2 moveInput;
    public float LastPressedJumped {  get; private set; }  
    #endregion

    #region Checks
    [Header("Checks")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    #endregion

    #region Layers & Tags
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask groundLayer;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //SetGravityScale(data.gravityScale);
        IsFacingRight = true;
    }

    private void Update()
    {
        #region Timers
        LastOnGroundTime -= Time.deltaTime;

        LastPressedJumped -= Time.deltaTime;
        #endregion

        #region Input manager 
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if(moveInput.x != 0)
        {
            //CheckDirectionToFace(moveInput.x);
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
            //OnJumpInput();
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
        {
            //OnJumpUpInput();
        }
        #endregion

        #region Collisons
        if(!isJumping)
        {
            if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer) && IsFacingRight)
            {
                LastOnGroundTime = data.coyteTime;
            }
        }
        #endregion
    }
    private void FixedUpdate()
    {
        Run();
    }

    private void Run()
    {

        float directionSpeed = moveInput.x * data.maxSpeed;

        #region Calculate AccelRate
        float acceRate;

        if(LastOnGroundTime > 0)
        {
            acceRate = (Mathf.Abs(directionSpeed) > 0.01f) ? data.accelAmount : data.deccelAmount;
        }
        else 
        {
            acceRate = (Mathf.Abs(directionSpeed) > 0.01f) ? data.accelAmount * data.accelAmount : data.deccelAmount * data.deccelInAir;
        }
        #endregion
    }

    //Movement
    //Jumping
    //Abilities
    //Climbing

}
