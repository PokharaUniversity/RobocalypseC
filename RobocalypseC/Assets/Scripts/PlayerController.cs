using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator Anim;
    [Range(0, 1)]
    public float LookDirection;
    public bool facingPositive;
    public float moveDirection;
    public float  WalkSpeed;
    public float RunSpeed;
    private float Speed;
    public float Gravity;
    public GameObject piviot;
    private CharacterController Controller;
    public float jumpForce;
    private float verticalVelocity;
    public float GroundClearance;
    private float inAirtime = 0;
    public bool is_inAir;
    float initialVelocity = 0;
    public float overheadClearance;
    public bool running;
    public bool movingForward;
    private GunPlaceHolder gunHolder;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gunHolder = GetComponentInChildren<GunPlaceHolder>();
        
    }




    // Update is called once per frame
    void Update()
    {
        move(); 
        point_at_mouse();
        //Debug.Log(verticalVelocity+" "+inAirtime);

      
        
    }





    void move() {
        Anim = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        //for jump,falling etc related to y axis
        if (GroundedCheck())
        {
            initialVelocity = 0;
            if (Input.GetAxis("Jump") == 1)
            {
                // Debug.Log("jump");
                initialVelocity = jumpForce;
                Anim.SetTrigger("Jump");
            }
            inAirtime = 0;
            is_inAir = false;

        }
        else
        {
            if (verticalVelocity > 0)
            {
                if (overhead_check())
                {
                    Debug.Log("struck"); ;
                    inAirtime = 0;
                    initialVelocity = 0;
                }
            }
            inAirtime += Time.deltaTime;
            is_inAir = true;
        }
        verticalVelocity = initialVelocity - Gravity * inAirtime;
        moveDirection = Input.GetAxis("Horizontal");
        Vector3 Direction = new Vector3(0, verticalVelocity, moveDirection * Speed) * Time.deltaTime;
        Controller.Move(Direction);
        //walking direction
        if ((moveDirection > 0 && facingPositive) || (moveDirection < 0 && !facingPositive))
        {
            movingForward = true;
        }
        else if ((moveDirection < 0 && facingPositive) || (moveDirection > 0 && !facingPositive))
        {
            movingForward = false;
            running = false;
        }

        //animation state
        int animationState;
        //Debug.Log(gunHolder.isShooting);
        if (movingForward && gunHolder.isShooting == false)
        {
            running = true;
        }
        else {
            running = false;
        }
        if (moveDirection == 0)
        {
            Speed = WalkSpeed;
            animationState = 1;
        }
        else if (movingForward == false)
        {
            Speed = WalkSpeed;
            animationState = 0;
            running = false;
        }
        else if (running == false)
        {
            Speed = WalkSpeed;
            animationState = 2;
        }
        else {
            animationState = 4;
            Speed = RunSpeed;
        }

        //movement animation
        float animationBlend = animationState;
        animationBlend /= 3;
        //Debug.Log(animationBlend);
        Anim.SetFloat("Speed", animationBlend);
    }







    bool overhead_check() {
        RaycastHit hit;
        bool isStruck=false;
        if (Physics.Raycast(piviot.transform.position, Vector3.up, out hit, overheadClearance))
        {
            if(!hit.collider.isTrigger)
            isStruck = true;
        }
        else {
            isStruck = false;
        }
        return isStruck;

       
    }

  



    bool GroundedCheck()
    {
        RaycastHit hit;
        bool isGrounded=false;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundClearance))
        {
            if(!hit.collider.isTrigger)
            isGrounded = true;
        }
      
        Anim.SetBool("Grounded", isGrounded);
        return isGrounded;
    }





    void point_at_mouse()
    {   
        Vector2 mouseposition = Input.mousePosition;
        Vector2 handScreenPosition = Camera.main.WorldToScreenPoint(piviot.transform.position);

        float angle = Vector2.Angle(mouseposition - handScreenPosition, new Vector2(1, 0));
        //Debug.Log(mouseposition + " " + handScreenPosition);
        //remapping angle to 0 to 1 from -90 to +90
        if (mouseposition.x > handScreenPosition.x)
        {
            facingPositive = true;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z)));
            if (mouseposition.y < handScreenPosition.y)
            {
                angle *= -1f;
            }
            angle += 90;
            angle /= 180;
        }
        else
        {
            facingPositive = false;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z)));
            angle -= 90;
            angle /= 90;
            angle = 1 - angle;
            if (mouseposition.y < handScreenPosition.y)
            {
                angle *= -1f;
            }
            angle /= 2;
            angle += 0.5f;
        }
        //Debug.Log(angle);
        Anim.SetFloat("lookDirection", angle);

        //Debug.Log (dir.x);

    }






    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawRay(piviot.transform.position + new Vector3(0, overheadClearance, 0), Vector3.up);
    }
}
