using UnityEngine;
using System.Collections;
using TMPro;

public class Attackable : MonoBehaviour
{
    public int HP = 40;
    public bool imortal;
    public GameObject hitNumberPrefab;
    public float numberSpawnHeight;

    public virtual void TakeDamage(int damage)
    {
        GameObject nbr = Instantiate(hitNumberPrefab, transform.position, Quaternion.identity);
        nbr.GetComponentInChildren<TMP_Text>().text = damage.ToString();
        nbr.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));

        StartCoroutine(DespawnNumber(nbr));

        if (imortal) return;

        HP -= damage;

        if (HP <= 0)
            Death();
    }

    public IEnumerator DespawnNumber(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        Destroy(obj);
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, numberSpawnHeight), .4f);
    }
}
