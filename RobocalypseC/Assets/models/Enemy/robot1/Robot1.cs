using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot1 : AIObjects
{
    private GunPlaceHolder GunPlaceHolder;
    private GameObject player;
    public gameManager Manager;
    [Header("Sensors")]
    public float detectRange;
    public float attackRange;
    public float GroundClearance;
    public float overheadClearance;
    [Header("Status")]
    public bool found;
    public bool is_inAir;
    [Header("Movement")]
    public float Speed;
    public float jumpForce;
    public bool movingPositive;
    public bool moving;
    public float rotationSmoother;
    public float targetSmoother;
    [Range(-1,1)]
    public float targetOffset;
    [Header("animation")]
    public Animator Anim;
    public Transform piviot;
    private float DistanceBetweenPlayerAndThis;
    private float initialVelocity;
    private float inAirTime;
    private float verticalVelocity;
    private float Gravity;
    private bool DontCheckGround=false;
    private CharacterController Controller;
    // Start is called before the first frame update
    void Start()
    {
        GunPlaceHolder = GetComponentInChildren<GunPlaceHolder>();
        player = Manager.Player;
        Gravity = Manager.Gravity;
        Controller = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        move();
        
        DistanceBetweenPlayerAndThis = Vector3.Distance(player.transform.position, transform.position);
        if (DistanceBetweenPlayerAndThis < detectRange)
        {
            if (DistanceBetweenPlayerAndThis < attackRange)
            {
                shoot();
            }
            else
            {
                chase();
            }
        }
        else {
            moving = false;
        }
        Anim.SetBool("isWalking", moving);
    }
    void move()
    {
        Anim = GetComponentInChildren<Animator>();
        //for jump,falling etc related to y axis
        if (GroundedCheck()&&!DontCheckGround)
        {
            initialVelocity = 0;
            inAirTime = 0;
            is_inAir = false;
        }
        else
        {
            if (verticalVelocity > 0)
            {
                if (overhead_check())
                {
                    inAirTime = 0;
                    initialVelocity = 0;
                }
            }
            inAirTime += Time.fixedDeltaTime;
            is_inAir = true;
        }
        if (movingPositive)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z), rotationSmoother * Time.fixedDeltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z), rotationSmoother * Time.fixedDeltaTime);
        }
        verticalVelocity = initialVelocity - Gravity * inAirTime;
        Vector3 Direction = new Vector3(0, verticalVelocity, (movingPositive?1:-1) *(moving?1:0)* Speed) * Time.deltaTime;
        Controller.Move(Direction);
        DontCheckGround = false;
        Anim.SetBool("isWalking",true);
    }
    public override void jump()
    {
        if (GroundedCheck()&&moving)
        {
            Debug.Log("called");
            initialVelocity = jumpForce;
            Anim.SetTrigger("Jump");
            DontCheckGround = true;
        }
        base.jump();
    }
    public override bool heading()
    {
        return movingPositive;
       
    }
    bool GroundedCheck()
    {
        RaycastHit hit;
        bool isGrounded = false;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundClearance))
        {
            if (!hit.collider.isTrigger)
                isGrounded = true;
        }

        Anim.SetBool("InAir", !isGrounded);
        return isGrounded;
    }
    void shoot() {
        //pointAtPlayer
        RaycastHit hit;
        Anim.SetFloat("lookDirection", Mathf.Lerp(Anim.GetFloat("lookDirection"), target() + targetOffset - targetOffset * (DistanceBetweenPlayerAndThis - attackRange) / DistanceBetweenPlayerAndThis, targetSmoother));
        moving = false;
        GunPlaceHolder.shoot();
    }
    float target() {
        float angle = Vector3.Angle(player.transform.position - transform.position, new Vector3(0, 0, 1));
        if (player.transform.position.z > transform.position.z)
        {
            movingPositive = true;
            if (player.transform.position.y < transform.position.y)
            {
                angle *= -1f;
            }
            angle += 90;
            angle /= 180;
        }
        else
        {
            movingPositive = false;
            angle -= 90;
            angle /= 90;
            angle = 1 - angle;
            if (player.transform.position.y < transform.position.y)
            {
                angle *= -1f;
            }
            angle /= 2;
            angle += 0.5f;
        }
        return angle;
    }
    void chase() {
        target();
        moving = true;
    }
    bool overhead_check()
    {
        RaycastHit hit;
        bool isStruck = false;
        if (Physics.Raycast(piviot.transform.position, Vector3.up, out hit, overheadClearance))
        {
            if (!hit.collider.isTrigger)
                isStruck = true;
        }
        else
        {
            isStruck = false;
        }
        return isStruck;


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(piviot.transform.position + new Vector3(0, overheadClearance, 0), Vector3.up);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        
    }
}
