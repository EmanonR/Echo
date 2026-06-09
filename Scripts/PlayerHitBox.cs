using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public int damage;

    public BoxCollider2D hitBox;

    private void Awake()
    {
        hitBox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attackable other = collision.GetComponent<Attackable>();

        if (other == null) return;

        other.TakeDamage(damage);
    }

    public void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public void DissableHitBox()
    {
        hitBox.enabled = false;
    }
}
