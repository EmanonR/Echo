using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] int maxJumps;

    public static float horizontal;

    public static event Action onJump;
    public static bool moveOverride;
    public static PlayerController Instance;
    PlayerCollisionHandler colHandler;

    int jumpCounter;
    Rigidbody2D rb;

    private void Awake()
    {
        Instance = this;
        colHandler = GetComponent<PlayerCollisionHandler>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (moveOverride) return;

        if (jumpCounter > 0 && Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void FixedUpdate()
    {

        if (colHandler.below) jumpCounter = maxJumps;
        Movement();
    }

    void Movement()
    {
        if (moveOverride) return;

        if (colHandler.below.collider == null) return;
        horizontal = Input.GetAxisRaw("Horizontal");

        if (colHandler.left.collider != null && horizontal < 0) return;
        if (colHandler.right.collider != null && horizontal > 0) return;

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
    }

    void Jump()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, jumpPower);
        StartCoroutine(RemoveJump());
        onJump?.Invoke();
    }

    public void OverrideVelocity(Vector2 newVel, bool right)
    {
        float xVel = right ? newVel.x : -newVel.x;

        rb.linearVelocity = new Vector3(xVel, newVel.y);
    }

    IEnumerator RemoveJump()
    {
        yield return new WaitForSeconds(.1f);
        jumpCounter--;
    }
}
