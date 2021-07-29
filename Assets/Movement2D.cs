using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    Rigidbody2D Rigid2D;
    Animator animator;


    bool isGround; //Is Ground?
    private int maxJump = 1;
    
    [SerializeField]
    //Dash Valuable
    private float moveSpeed = 2.0f;//Power of speed
    [SerializeField]
    //Jump Valuable
    private float jumpForce = 4.0f;
    [SerializeField]
    public Transform pos; //position
    [SerializeField]
    public float checkRadius; //Make circle (Radius)
    [SerializeField]
    public LayerMask islayer; //Found layer, return True
    [SerializeField]
    int jumpCount; //Jump count variable
    [SerializeField]
    float ST_Dash; //Dash sustainment time variable
    [SerializeField]
    float DashCooltime = 3.0f; //Dash Cooltime variable
    [SerializeField]
    bool isDash = true; // is Dash?
    [SerializeField]
    bool isMove; //is Move?
    
    //CapsuleCollider2D
        void Start()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = maxJump; //점프카운트를 최대점프횟수로 초기화
    }

    void Update()
    {
        move();
        Jump();
        Dash();
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(pos.position, checkRadius, islayer);
    }

    public void move()
    {

        float x = Input.GetAxisRaw("Horizontal");
        Rigid2D.velocity = new Vector2(x*moveSpeed, Rigid2D.velocity.y);
        
        //Direction of sprite
        if(x > 0)
        {
            Vector2 Scale = transform.localScale;
            Scale.x = Mathf.Abs(Scale.x); //Mathf.Abs = 절댓값
            transform.localScale = Scale;
            animator.SetBool("isMove",true);
            isMove = true;
        }
        else if(x < 0)
        {
            Vector2 Scale = transform.localScale;
            Scale.x = -Mathf.Abs(Scale.x);
            transform.localScale = Scale;
            animator.SetBool("isMove",true);
            isMove = true;
        }
        else if(x == 0)
        {
            animator.SetBool("isMove",false);
            isMove = false;
        }

    }

    public void Jump()
    {
        isGround = Physics2D.OverlapCircle(pos.position, checkRadius, islayer);

        if(isGround == true)
        {
            jumpCount = maxJump;
            animator.SetBool("isJump",false);
        }
        else
        {
            animator.SetBool("isJump",true);
        }
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            Rigid2D.velocity=Vector2.zero; //Same Jump
            Rigid2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;

        }
    }
    void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pos.position,checkRadius);
    }
    void Dash()
    {
        if(isMove == true)
        {
            if(isDash == true)
            {
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    ST_Dash = 3.0f;
                    moveSpeed = 30;
                    DashCooltime = 3.0f;
                    isDash = false;
                    StartCoroutine("ST_DashCool");
                    StartCoroutine("DashCool");
                }
            }
        }
    }
    
    IEnumerator ST_DashCool() //대쉬지속시간
    {
        if(ST_Dash > 0)
        {
            ST_Dash -= 1.0f;
            yield return new WaitForSeconds(0.1f); //0.1초 동안 moveSpeed = 30
        }
        moveSpeed = 3.0f;
        yield break;
    }
    IEnumerator DashCool() //대쉬 재사용시간
    {
        if(DashCooltime > 0)
        {
            DashCooltime -= 1.0f;
            yield return new WaitForSeconds(3.0f); //대쉬 쿨타임이 3초
        }
        isDash = true;
        yield break;
    }
}
