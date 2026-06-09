using UnityEngine;

public class Attackable : MonoBehaviour
{
    public int HP = 40;
    public bool imortal;

    public virtual void TakeDamage(int damage)
    {
        if (imortal) return;

        HP -= damage;

        if (HP <= 0)
            Death();
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
