using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public List<Effects> effect;
    public Vector2 OverrideVel;
    public int damage = 10;

    [Header("HitBox")]
    public Vector2 offset = new();
    public Vector2 size = new(1, 1);
}

[System.Serializable]
public enum Effects
{
    VelocityOverride,
    CantMove
}
