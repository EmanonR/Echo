using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttacker : MonoBehaviour
{
    public List<Combo> combos;
    float lastAttackedTime;
    float lastComboEnd;
    int comboCounter;
    int currentCombo;

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
        {
            currentCombo = ValidateAndFindAttack();

            if (currentCombo != -1)
                Attack();
        }

        ExitAttack();
    }

    int ValidateAndFindAttack()
    {
        //cycle all combos
        for (int i = 0; i < combos.Count; i++)
        {
            //Check for matching requirements
            bool validation = CheckRequirements(combos[i].activationReq);

            if (validation)
                return i;
        }

        return -1;
    }

    bool CheckRequirement(Requirement req)
    {
        switch (req)
        {
            case Requirement.Grounded:
                if (PlayerController.below)
                    return true;
                break;

            case Requirement.AirBorn:
                if (!PlayerController.below)
                    return true;
                break;

            case Requirement.Moving:
                if (PlayerController.horizontal != 0)
                    return true;
                break;
        }

        return false;
    }

    bool CheckRequirements(List<Requirement> req)
    {
        bool val = true;

        for (int i = 0; i < req.Count; i++)
        {
            if (CheckRequirement(req[i]) == false)
            {
                val = false;
                break;
            }
        }

        return val;
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > .1f && comboCounter < combos[currentCombo].attacks.Count)
        {
            CancelInvoke(nameof(EndCombo));

            if (Time.time - lastAttackedTime >= .1f)
            {
                AnimatorController.attacking = true;
                print("ATTACK!");
                animator.runtimeAnimatorController = combos[currentCombo].attacks[comboCounter].animatorOV;
                AnimatorController.PlayAnimation("Attack");
                hitBox.damage = combos[currentCombo].attacks[comboCounter].damage;
                comboCounter++;
                lastAttackedTime = Time.time;

                if (comboCounter > combos[currentCombo].attacks.Count)
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

[System.Serializable]
public class Combo
{
    public List<Requirement> activationReq;
    public List<AttackSO> attacks;
}

public enum Requirement
{
    Grounded,
    AirBorn,
    Moving
}
