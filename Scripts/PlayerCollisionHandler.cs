using UnityEngine;
using System;
using System.Collections;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] float groundedRayHeight;
    [SerializeField] float groundedRayLength;
    [SerializeField] LayerMask groundedRayMask;

    [Header("Walls")]
    [SerializeField] float sideRayBot;
    [SerializeField] float sideRayTop, sideRayWidth, sideRayLength;
    [SerializeField] LayerMask sideRayMask;

    public RaycastHit2D below, left, right;

    private void FixedUpdate()
    {
        below = CheckColl(transform.position, groundedRayLength, new Vector2(0, groundedRayHeight), Vector2.down, groundedRayMask);
        left = CheckColl(transform.position, sideRayLength, new Vector2(sideRayWidth, sideRayBot), Vector2.left, sideRayMask);
        right = CheckColl(transform.position, sideRayLength, new Vector2(-sideRayWidth, sideRayBot), Vector2.right, sideRayMask);
    }

    public static RaycastHit2D CheckColl(Vector2 position, float length, Vector2 offset, Vector2 dir, LayerMask mask)
    {
        return Physics2D.Raycast(position + offset, dir, length, mask);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 groundedPos = new (transform.position.x, transform.position.y + groundedRayHeight);

        Vector2 LB = new (transform.position.x + sideRayWidth, transform.position.y + sideRayBot);
        Vector2 LT = new (transform.position.x + sideRayWidth, transform.position.y + sideRayTop);
        Vector2 RB = new (transform.position.x - sideRayWidth, transform.position.y + sideRayBot);
        Vector2 RT = new (transform.position.x - sideRayWidth, transform.position.y + sideRayTop);


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
