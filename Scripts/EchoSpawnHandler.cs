using UnityEngine;

public class EchoSpawnHandler : MonoBehaviour
{
    public GameObject echoPrefab;

    private void Awake()
    {
        PlayerAttacker.onAttack += SpawnEcho;
    }

    public void SpawnEcho(Vector2 position, AnimationClip anim, Attack attack)
    {
        GameObject echo = Instantiate(echoPrefab, position, Quaternion.identity);
        EchoHandler handler = echo.GetComponent<EchoHandler>();

        handler.currentAttack = attack;
        handler.velocity = attack.attack.OverrideVel;
        handler.damage = attack.attack.damage / 2;
        handler.anim = anim;
    }
}
