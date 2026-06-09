using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public Animation animation;
    public int damage = 10;
}
