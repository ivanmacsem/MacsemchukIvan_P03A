using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 10f;
    public float jumpSpeed = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float slideSpeed = 3f;
    public ParticleSystem dust;

    public LayerMask groundLayer;
    public Transform groundCheckCol;
    public Transform leftCheckCol;
    public Transform rightCheckCol;
    public bool grounded = false;
    public bool walled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x,y);

        GroundCheck();
        WallCheck();
        Walk(dir);
        Jump();
        if(walled && !grounded){
            WallSlide();
        }
        if(walled && Input.GetKey(KeyCode.LeftShift)){
            rb.velocity = new Vector2(rb.velocity.x, y*speed);
        }
        if(grounded && x!=0){
            GetComponent<AudioSource>().Play();
        }
    }

    void CreateDust(){
        dust.Play();
    }

    private void GroundCheck(){
        grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCol.position,0.2f, groundLayer);
        if(colliders.Length>0){
            grounded = true;
        }
    }

    private void WallCheck(){
        walled = false;
        Collider2D[] leftColliders = Physics2D.OverlapCircleAll(leftCheckCol.position, 0.2f, groundLayer);
        Collider2D[] rightColliders = Physics2D.OverlapCircleAll(rightCheckCol.position, 0.2f, groundLayer);
        if(leftColliders.Length>0 || rightColliders.Length>0){
            walled = true;
        }
    }

    private void Walk(Vector2 dir){
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
    }

    private void Jump(){
        if(grounded && Input.GetButtonDown("Jump")){
            rb.velocity = Vector2.up * jumpSpeed;
            CreateDust();
        }
        if(rb.velocity.y < 0){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier -1) * Time.deltaTime;
        }
    }

    private void WallSlide(){
        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
    }
}
