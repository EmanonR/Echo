using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] int maxJumps;

    [Header("Ground")]
    [SerializeField] float groundedRayHeight;
    [SerializeField] float groundedRayLength;
    [SerializeField] LayerMask groundedRayMask;

    [Header("Walls")]
    [SerializeField] float sideRayBot;
    [SerializeField] float sideRayTop, sideRayWidth, sideRayLength;
    [SerializeField] LayerMask sideRayMask;

    public static float horizontal;

    public static RaycastHit2D below, left, right;

    public static event Action onJump;

    int jumpCounter;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (jumpCounter > 0 && Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void FixedUpdate()
    {
        below = CheckColl(groundedRayLength, new Vector2(0, groundedRayHeight), Vector2.down, groundedRayMask);
        left = CheckColl(sideRayLength, new Vector2(sideRayWidth, sideRayBot), Vector2.left, sideRayMask);
        right = CheckColl(sideRayLength, new Vector2(-sideRayWidth, sideRayBot), Vector2.right, sideRayMask);

        if (below) jumpCounter = maxJumps;
        Movement();
    }

    RaycastHit2D CheckColl(float length, Vector2 offset, Vector2 dir, LayerMask mask)
    {
        return Physics2D.Raycast(transform.position + (Vector3)offset, dir, length, mask);
    }

    void Movement()
    {
        if (below.collider == null) return;
        horizontal = Input.GetAxisRaw("Horizontal");

        if (left.collider != null && horizontal < 0) return;
        if (right.collider != null && horizontal > 0) return;

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
    }

    void Jump()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, jumpPower);
        StartCoroutine(RemoveJump());
        onJump?.Invoke();
    }

    IEnumerator RemoveJump()
    {
        yield return new WaitForSeconds(.1f);
        jumpCounter--;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 groundedPos = new Vector2(transform.position.x, transform.position.y + groundedRayHeight);

        Vector2 LB = new Vector2(transform.position.x + sideRayWidth, transform.position.y + sideRayBot);
        Vector2 LT = new Vector2(transform.position.x + sideRayWidth, transform.position.y + sideRayTop);
        Vector2 RB = new Vector2(transform.position.x - sideRayWidth, transform.position.y + sideRayBot);
        Vector2 RT = new Vector2(transform.position.x - sideRayWidth, transform.position.y + sideRayTop);


        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(groundedPos, .1f);

        Gizmos.DrawWireSphere(LB, .1f);
        Gizmos.DrawWireSphere(LT, .1f);
        Gizmos.DrawWireSphere(RB, .1f);
        Gizmos.DrawWireSphere(RT, .1f);


        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundedPos, Vector3.down * groundedRayLength);

        Gizmos.DrawRay(LB, Vector3.left * sideRayLength);
        Gizmos.DrawRay(LT, Vector3.left * sideRayLength);
        Gizmos.DrawRay(RB, Vector3.right * sideRayLength);
        Gizmos.DrawRay(RT, Vector3.right * sideRayLength);
    }
}
