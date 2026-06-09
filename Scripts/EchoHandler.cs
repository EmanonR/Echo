using UnityEngine;
using System.Collections.Generic;

public class EchoHandler : MonoBehaviour
{
    public AnimationClip anim;
    public PlayerHitBox hitBox;
    public Vector2 velocity;
    public int damage;
    public float waitTime = 1f;
    public float timer;

    public Attack currentAttack;
    Animator animator;
    PlayerCollisionHandler colHandler;
    Rigidbody2D rb;

    public bool lookingRight;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        colHandler = GetComponentInChildren<PlayerCollisionHandler>();
        rb = GetComponentInChildren<Rigidbody2D>();
        timer = waitTime;
    }

    private void Update()
    {
        hitBox.damage = damage;
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            PlayAttack();

            ExitAttack();
        }
    }

    public void PlayAttack()
    {
        hitBox.EnableHitBox();
        rb.WakeUp();
        rb.linearVelocity = new(lookingRight ? velocity.x : -velocity.x, velocity.y);
        AnimatorOverrideController ov = new(animator.runtimeAnimatorController);
        ov["Attack"] = anim;

        GetComponentInChildren<SpriteRenderer>().flipX = !lookingRight;

        animator.runtimeAnimatorController = ov;
        animator.Play("Attack");
    }


    bool CheckCancelCon(CancelationCon cancelCon)
    {
        switch (cancelCon)
        {
            case CancelationCon.Grounded:
                if (colHandler.below)
                    return true;
                break;

            case CancelationCon.HitWall:
                if (lookingRight)
                {
                    if (colHandler.right)
                        return true;
                }
                else
                {
                    if (colHandler.left)
                        return true;
                }
                break;
        }

        return false;
    }

    bool CheckCancelCons(List<CancelationCon> cancelCons)
    {
        bool eval = true;

        for (int i = 0; i < cancelCons.Count; i++)
        {
            if (CheckCancelCon(cancelCons[i]) == false)
            {
                eval = false;
                break;
            }
        }

        return eval;
    }

    void ExitAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .99f && currentAttack.CancelOnAnimEnd)
            Dissable();

        if (currentAttack.cancelationCon.Count == 0)
            return;
        else if (CheckCancelCons(currentAttack.cancelationCon))
            Dissable();
    }

    void Dissable()
    {
        Destroy(gameObject);
    }
}
