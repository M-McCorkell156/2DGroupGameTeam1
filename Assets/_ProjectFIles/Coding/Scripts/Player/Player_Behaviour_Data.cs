using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will create a new playerData object
[CreateAssetMenu(menuName = "Player Behaviour Data")] 

public class Player_Behaviour_Data : ScriptableObject
{

    [Header("Gravity")]

    [HideInInspector] public float gravityPower; 
    [HideInInspector] public float gravityScale; 

    [Space(5)]
    public float fallGravityMult; 
    public float maxFallSpeed; 
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
                                      //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.

    [Space(20)]

    [Header("Movement")]
    
    public float maxSpeed;

    //Time it takes for the player to accelerate from 0 to the maxSpeed and also decelerate from maxSpeed to 0
    public float timeToAccelerate;
    public float timeToDecceleration; 

    //Final force that is applied to the player
    [HideInInspector] public float accelAmount; 
    [HideInInspector] public float deccelAmount; 

    [Space(10)]

    //Multipliers applied to acceleration rate when in the air.
    [Range(0.01f, 1)] public float accelInAir; 
    [Range(0.01f, 1)] public float deccelInAir;
    public bool doConserveMomentum;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; //Height of the player's jump
    public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases thje jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangTimeThreshold; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    
    [Space(0.5f)]

    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityPower = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityPower / Physics2D.gravity.y;

        jumpForce = Mathf.Abs(gravityPower) * jumpTimeToApex;

        //Calculating the acceleration & deceleration forces
        accelAmount = (50 * timeToAccelerate) / maxSpeed;
        deccelAmount = (50 * timeToDecceleration) / maxSpeed;

        #region Variable Ranges
        timeToAccelerate = Mathf.Clamp(timeToAccelerate, 0.01f, maxSpeed);
        timeToDecceleration = Mathf.Clamp(timeToDecceleration, 0.01f, maxSpeed);
        #endregion
    }
}
