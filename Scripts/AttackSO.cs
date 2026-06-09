using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public List<Effects> effect;
    public int damage = 10;
}

[System.Serializable]
public enum Effects
{
    VelocityOverride,
    CantMove
}
