using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private GameObject parent;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    private enum State { idle, running, jumping, falling, hurt };
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int points = 0;
    [SerializeField] private Text pointText;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (state != State.hurt)
        {
            MovementController();
        }
        VelocityState();
        anim.SetInteger("state", (int)state);
    }

    private void MovementController()
    {
        float xDirection = Input.GetAxis("Horizontal");

        if (xDirection < 0)
        {
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        else if (xDirection > 0)
        {
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collectable")
        {
            Destroy(collision.gameObject);
            points += 1;
            pointText.text = points.ToString();
        }

    }

    private void VelocityState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 1)
        {
            state = State.running;
        }

        else
        {
            state = State.idle;
        }
    }
}
