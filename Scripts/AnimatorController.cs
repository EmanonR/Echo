using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public static Animator animator;

    Rigidbody2D rb;
    SpriteRenderer spriteRen;

    public static string currentAnimName;
    string prevAnimName;

    public static bool lookingRight;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        spriteRen = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (rb.linearVelocityX != 0)
        {
            lookingRight = rb.linearVelocityX > 0;
            spriteRen.flipX = rb.linearVelocityX < 0;
        }


        if (PlayerAttacker.attacking) return;


        if (PlayerController.below) //Grounded
        {
            if (PlayerController.horizontal != 0)
                PlayAnimation("Movement");
            else
                PlayAnimation("Idle");
        }
        else
        {
            if (rb.linearVelocityY > 0)
            {
                PlayAnimation("StartJump");
            }
            else if (rb.linearVelocityY < 0)
            {
                PlayAnimation("JumpTransition");
            }
        }
    }

    public static void PlayAnimation(string name)
    {
        if (currentAnimName == name) return;

        currentAnimName = name;
        animator.Play(name, 0);
    }
}
