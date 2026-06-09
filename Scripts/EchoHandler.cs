using UnityEngine;

public class EchoHandler : MonoBehaviour
{
    public AnimationClip anim;
    public PlayerHitBox hitBox;
    public Vector2 velocity;
    public int damage;
    public float waitTime = 1f;
    public float timer;

    Animator animator;
    PlayerCollisionHandler colHandler;
    Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        colHandler = GetComponentInChildren<PlayerCollisionHandler>();
        rb = GetComponentInChildren<Rigidbody2D>();
        timer = waitTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            PlayAttack();

            ValidateExit();
        }
    }

    public void PlayAttack()
    {
        hitBox.EnableHitBox();
        rb.WakeUp();
        rb.linearVelocity = velocity;
        AnimatorOverrideController ov = new(animator.runtimeAnimatorController);
        ov["Attack"] = anim;

        animator.runtimeAnimatorController = ov;
        animator.Play("Attack");
    }

    void ValidateExit()
    {

    }

    void Dissable()
    {
        Destroy(gameObject);
    }
}
