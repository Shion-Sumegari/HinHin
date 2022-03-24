using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float moveInput;

    private Animator animator;
    private Rigidbody2D rb;

    private bool facingRight = true;

    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJump;
    public int extraJumpValue;

    public float wallJumpTime = 0;
    public float wallSlideSpeed = 0.5f;
    public float wallDistance = 0.5f;

    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;

    public ParticleSystem dust;
    // Start is called before the first frame update
    void Start()
    {
        Flip();
        extraJump = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        moveInput = Input.GetAxisRaw("Horizontal")*speed;
        Dust();
        rb.velocity = new Vector2(moveInput, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if(!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput <0)
        {
            Flip();
        }
        if (facingRight)
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, whatIsGround);
        }
        else
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, whatIsGround);
        }

        if(WallCheckHit && !isGrounded && moveInput != 0)
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }else if(jumpTime < Time.time)
        {
            isWallSliding = false;
        }
        if (isWallSliding)
        {
            Dust();
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
            animator.SetBool("Wall Jump", true);

        }
        else
        {
            animator.SetBool("Wall Jump", false);
        }
    }
    private void Update()
    {
        if (isGrounded)
        {
            extraJump = extraJumpValue;
            animator.SetBool("Jump", false);
            animator.SetBool("Double Jump", false);
            animator.SetInteger("State", 0);

        } else if (!isGrounded)
        {
            animator.SetBool("Jump", true);
            animator.SetFloat("Speed", 0f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
        {
            if (isGrounded) { Dust(); }
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("Jump", false);
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Double Jump", true);
            
            extraJump--;
        } else if (Input.GetKeyDown(KeyCode.Space)&& extraJump == 0 && isGrounded)
        {
            Dust();
            rb.velocity = Vector2.up * jumpForce;
            animator.SetInteger("State", 1);
        }
         if(!isGrounded && rb.velocity.y < 0)
        {
            if(extraJump == 0)
            {
                animator.SetBool("Double Jump", false);
            }
            animator.SetInteger("State", 1);
            
        }
         if(!isGrounded && rb.velocity.y > 0)
        {
            if(extraJump > 0)
            {
                animator.SetBool("Double Jump", false);
            }
        }
    }
    void Flip()
    {
        Dust();
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void Dust()
    {
        dust.Play();
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
