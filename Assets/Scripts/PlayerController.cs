using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour, InputControls.IMovementActions, InputControls.IShootingActions
{
    private InputControls ctrl;
    private Rigidbody2D rb;
    private Vector2 boxSize = new Vector2(0.16f, 0.16f);

    //Running

    private Vector2 inputMove;
    public float speed = 2.5f;
    public float maxSpeed = 5f;
    [Range(0, 0.3f)] [SerializeField] private float smoothTime = 0.05f;
    private Vector2 m_Velocity = Vector2.zero;


    //GroundControl

    public LayerMask whatIsGround;

    public Vector2 debug;


    //Dashing

    public float dashPower = 200f;
    public int maxDashCount = 2;
    private int currDashCount;

    //Jumping and Falling

    public int jumpPower = 10;
    public int maxJumpCount = 2;
    private int currJumpCount;

    public float fallMult = 2.5f;
    public float lowFallMult = 3f;


    //Aiming

    public GameObject aimPoint;
    public float aimDistance = 2f;
    private Vector2 aimDirection = Vector2.right;
    private float angle;
    private Quaternion aimOrientation;


    //Shooting

    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;


    void Awake()
    {
        ctrl = new InputControls();
        ctrl.Movement.SetCallbacks(this);
        ctrl.Shooting.SetCallbacks(this);
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        Move(inputMove.x * Time.fixedDeltaTime);
        Grounder();
    }
    private void Update()
    {
        GravityCheck();
        Aim();
        HowToRotate();
    }


    void OnEnable()
    {
        ctrl.Enable();
    }
    void OnDisable()
    {
        ctrl.Disable();
    }

    public void OnDirection(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>() * speed;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GameObject bullet = Instantiate(bulletPrefab, aimPoint.transform.position, aimOrientation);
            bullet.GetComponent<Rigidbody2D>().AddForce(aimDirection * bulletSpeed);
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aimDirection = context.ReadValue<Vector2>();
            angle = Vector2.SignedAngle(Vector2.right, aimDirection);
            aimOrientation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Dash();
        }
    }



    void GravityCheck()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && ctrl.Movement.Jump.ReadValue<float>() == 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowFallMult - 1) * Time.deltaTime;
        }
    }
    void Jump()
    {
        currJumpCount++;
        if (currJumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPower));
        }
    }
    void Move(float move)
    {
        Vector2 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, smoothTime, maxSpeed);
    }
    void Aim()
    {
        aimPoint.transform.position = aimDirection * aimDistance + (Vector2)transform.position;
        Quaternion lampAdjust = Quaternion.Euler(0, 0, aimOrientation.eulerAngles.z - 90);
        aimPoint.transform.rotation = lampAdjust;
    }
    void Dash()
    {
        currDashCount++;
        if (currDashCount < maxDashCount)
        {
            //vllt ein gravity oder velocity reset?
            rb.AddForce(aimDirection * dashPower);
        }

    }
    void Grounder()
    {
        if(Physics2D.OverlapBox(transform.position, boxSize, 0f, whatIsGround))
        {
            currDashCount = 0;
            currJumpCount = 0;
        }
    }
    void HowToRotate()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, 5000f, whatIsGround);
        debug = hit.normal;
    }
}
