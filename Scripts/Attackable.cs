using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Attackable : MonoBehaviour
{
    public int HP = 40;
    public bool imortal;
    public GameObject hitNumberPrefab;
    public float numberSpawnHeight;

    public List<GameObject> numbersPool;
    int nbrInd;

    private void Awake()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject nbr = Instantiate(hitNumberPrefab, transform.position, Quaternion.identity);
            numbersPool.Add(nbr);

            nbr.SetActive(false);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        GameObject nbr = numbersPool[nbrInd];
        nbr.GetComponentInChildren<TMP_Text>().text = damage.ToString();
        nbr.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));
        StartCoroutine(DespawnNumber(nbr));
        nbrInd++;

        if (imortal) return;

        HP -= damage;

        if (HP <= 0)
            Death();
    }

    public IEnumerator DespawnNumber(GameObject nbr)
    {
        yield return new WaitForSeconds(1f);
        nbr.SetActive(false);
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
