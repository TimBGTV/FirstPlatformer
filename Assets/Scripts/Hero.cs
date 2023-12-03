using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 15f;
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private States State
    { 
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    { 
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Run()
    {
        if (isGrounded)
            State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0;
        //rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);
    }

    private void Jump()
    {
        if (!isGrounded)
            State = States.jump;

        //rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
    }

    // Перечисление для анимации.
    public enum States
    { 
        idle,
        run,
        jump
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
            State = States.idle;
        
        if (Input.GetButton("Horizontal"))
            Run();
        

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
}
