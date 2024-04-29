using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Behaviour : MonoBehaviour
{
    //Scriptable object which holds all the player's movement parameters.
    public Player_Behaviour_Data Data;

    #region Variables
    public Rigidbody2D RB { get; private set; }
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }

    private bool isDead;

    private bool isRunning;

    //Locking
    private bool lockMove;
    //Timers
    public float LastOnGroundTime { get; private set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    private Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }

    //Chute
    private bool _haveChute;
    private bool _isChuting;

    //Sticky roof 
    private bool _haveSticky;
    private bool _isStickng;

    //Pick Ups
    private Collider2D pickUpname;

    [Space(5)]

    [Header("Objects")]

    //Spawning/Dying
    private GameObject SpawnPoint;
    [SerializeField] private GameObject spawnPoint;
    private Collider2D checkpointCollider;
    public float spawnTime;
    public GameObject nextSceneObj;

    //Chase 
    [SerializeField] private GameObject chaseEnemy;

    [Space(5)]

    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

    [SerializeField] private Transform _roofCheckPoint;
    [SerializeField] private Vector2 _roofCheckSize = new Vector2(0.49f, 0.03f);

    [Space(5)]

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _stickyRoofLayer;
    [SerializeField] private LayerMask _deathLayer;
    [SerializeField] private LayerMask _checkPointLayer;
    [SerializeField] private LayerMask _pickUpLayer;



    [Space(5)]

    [Header("Animator")]
    [SerializeField] private Animator animator;


    #region LedgeStuff
    [Header("Ledge Info")]
    [HideInInspector] public bool ledgeDetected;

    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBegunPos;
    private Vector2 climbOverPos;

    private bool canGrabLedge = true;
    private bool canClimb;
    [SerializeField] private float allowLedgeTime;

    #endregion

    #endregion

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        SpawnPoint = GameObject.Find("Spawn_Area");
        _haveSticky = false;
        _haveChute = false;
        
    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
        
    }

    private void Update()
    {
        if (!lockMove)
        {
            #region TIMERS
            LastOnGroundTime -= Time.deltaTime;


            LastPressedJumpTime -= Time.deltaTime;
            #endregion

            #region INPUT HANDLER
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");

            if (_moveInput.x != 0)
            {
                CheckDirectionToFace(_moveInput.x > 0);
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
            {
                OnJumpInput();
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
            {
                OnJumpUpInput();
            }

            #endregion

            #region COLLISION CHECKS

            if (!IsJumping)
            {
                //Ground Check
                if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
                {
                    LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
                    _isChuting = false;
                }
            }
            //Sticking Check
            if (Physics2D.OverlapBox(_roofCheckPoint.position, _roofCheckSize, 0, _stickyRoofLayer) && _haveSticky)
            {
                _isStickng = true;
            }
            else
            {
                _isStickng = false;
            }

            //Death Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _deathLayer))
            {
                //Debug.Log("Die Time");
                Death();
            }

            //Checkpoint Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _checkPointLayer))
            {
                //Debug.Log("Spawn time");
                checkpointCollider = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _checkPointLayer);
                SpawnPoint.transform.position = checkpointCollider.transform.position;
                //Chase scene 
                if (checkpointCollider.tag == "Chase")
                {
                    //Debug.Log("Chase");
                    chaseEnemy.GetComponent<Chase_Scene_Manager>().BeginChase();
                }
                else if (checkpointCollider.tag == "NextScene")
                {
                    nextSceneObj.GetComponent<Scene_Manager>().LoadScene();
                }
            }

            //Pickup Check
            if (Physics2D.OverlapBox(RB.position, RB.transform.localScale, 0, _pickUpLayer))
            {
                //Debug.Log("Pickup");
                pickUpname = Physics2D.OverlapBox(RB.position, RB.transform.localScale, 0, _pickUpLayer);
                if (pickUpname.tag == "StickyPickup")
                {
                    _haveSticky = true;
                    Destroy(pickUpname.gameObject);
                    //Debug.Log("Stick");

                }
                else if (pickUpname.tag == "ChutePickup")
                {
                    _haveChute = true;
                    //Debug.Log("Chute");
                    Destroy(pickUpname.gameObject);

                }
            }

            #endregion

            #region JUMP CHECKS
            if (RB.velocity.y < 0)
            {
                _isJumpFalling = true;
            }
            if (IsJumping && RB.velocity.y < 0)
            {
                IsJumping = false;
            }

            if (LastOnGroundTime > 0 && !IsJumping)
            {
                _isJumpCut = false;

                if (!IsJumping)
                {
                    _isJumpFalling = false;
                }
            }
            //Chute
            if (CanChute() && LastPressedJumpTime > 0)
            {
                _isChuting = true;
            }

            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                _isJumpFalling = false;
                Jump();
            }

            if (_isStickng && LastPressedJumpTime > 0)
            {
                _isStickng = false;
                //Debug.Log("Stop Sticking");
            }


            #endregion

            #region GRAVITY
            //Higher gravity if we've released the jump input or are falling
            if (_isStickng)
            {
                SetGravityScale(0);
                //Debug.Log("Sticking");
            }

            else if (_isChuting)
            {
                SetGravityScale(Data.realChuteGravity);
                //Debug.Log("Chuting");
            }

            else if (RB.velocity.y < 0 && !_isJumpCut)
            {
                //Much higher gravity if holding down
                //Debug.Log("Button Hold dowm");
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);

                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //Higher gravity if jump button released
                //Debug.Log("Button Let go");

                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //Higher gravity if falling
                Debug.Log("Falling");
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //Default gravity if standing on a platform or moving upwards
                SetGravityScale(Data.gravityScale);
            }
            #endregion

            AnimationController();

            CheckForLedge();
        }
    }

    private void FixedUpdate()
    {
        Run(1);
    }

    #region INPUT CALLBACKS
    //Methods which whandle input detected in Update()
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut())
        {
            _isJumpCut = true;
            //Debug.Log("Can jump cut");
        }

    }
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    public void lockMovement()
    {
        //Debug.Log("lockmove");
        _moveInput = Vector2.zero;
        SetGravityScale(0);
        RB.constraints = RigidbodyConstraints2D.FreezePosition;
        RB.velocity = Vector2.zero;
        lockMove = true;
    }
    public void unlockMovement()
    {
        //Debug.Log("unlockmove");
        _moveInput = Vector2.zero;
        RB.constraints = RigidbodyConstraints2D.None;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetGravityScale(Data.gravityScale);
        lockMove = false;
    }


    public void Death()
    {
        //Put Death animation Here 
        lockMovement();
        isDead = true;
        gameObject.transform.parent = null;
        //Fade Out maybe use method

        chaseEnemy.GetComponent<Chase_Scene_Manager>().ResetChase();
        
        //Fade In
    }

    public void Spawning()
    {
        isDead = false;
        RB.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 2);
        
        //Debug.Log("Spawn delay start");
        //StartCoroutine(SpawnDelay());
        Invoke(nameof(unlockMovement), spawnTime);

    }

    #endregion

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

        /*
         * For those interested here is what AddForce() will do
         * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
         * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
        */
    }

    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    #endregion

    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
        {
            if (!canClimb)
            {
                Turn();
            }
        }


    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping && !_isChuting && !_isStickng && !canClimb;
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanChute()
    {
        return ((_isJumpFalling || IsJumping) && !_isStickng && _haveChute && !canClimb);
    }



    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_roofCheckPoint.position, _roofCheckSize);
        Gizmos.color = Color.blue;
    }
    #endregion
    //Movement
    //Jumping
    //Abilities
    //Climbing

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnTime);
        //Debug.Log("Spawn delay done");
        unlockMovement();
    }

    #region Animations
    private void AnimationController()
    {
        animator.SetBool("IsChuting", _isChuting);
        animator.SetBool("IsJumping", IsJumping);
        animator.SetBool("IsFalling", _isJumpFalling);
        animator.SetBool("IsWalking", isRunning);
        animator.SetBool("CanClimb", canClimb);
        animator.SetBool("Death", isDead);
    }
    #endregion
    #region Ledges
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {

            canGrabLedge = false;
            canClimb = true;
            Vector2 ledgePos = GetComponentInChildren<Ledge_Detection>().transform.position;

            if (IsFacingRight)
            {
                climbBegunPos = ledgePos + offset1;
                climbOverPos = ledgePos + offset2;
            }
            else
            {
                climbBegunPos = ledgePos + -offset1;
                climbOverPos = ledgePos + -offset2;
            }
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
        Invoke(nameof(AllowLedgeGrab), allowLedgeTime);
    }
    private void AllowLedgeGrab() => canGrabLedge = true;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Twig") && (ledgeDetected && canGrabLedge))
    //    {
    //        canClimb = true;
    //        Debug.Log("CANCLIMB");
    //    }
    //}
    #endregion

}
