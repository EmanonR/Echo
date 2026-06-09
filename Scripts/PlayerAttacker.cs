using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttacker : MonoBehaviour
{
    public List<AttackSO> Combo;
    float lastAttackedTime;
    float lastComboEnd;
    int comboCounter;

    public KeyCode attackKey = KeyCode.J;

    Animator animator;
    [SerializeField] PlayerHitBox hitBox;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
            Attack();

        ExitAttack();
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > .1f && comboCounter < Combo.Count)
        {
            CancelInvoke(nameof(EndCombo));

            if (Time.time - lastAttackedTime >= .1f)
            {
                AnimatorController.attacking = true;
                print("ATTACK!");
                animator.runtimeAnimatorController = Combo[comboCounter].animatorOV;
                AnimatorController.PlayAnimation("Attack");
                hitBox.damage = Combo[comboCounter].damage;
                comboCounter++;
                lastAttackedTime = Time.time;

                if (comboCounter > Combo.Count)
                    comboCounter = 0;
            }
        }
    }

    void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .99f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke(nameof(EndCombo), 0);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        AnimatorController.attacking = false;
    }
}
