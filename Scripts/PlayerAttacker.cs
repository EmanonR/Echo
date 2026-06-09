using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttacker : MonoBehaviour
{
    public List<Attack> attacks;
    float lastAttackedTime;
    int attackInd;

    public KeyCode attackKey = KeyCode.J;

    Animator animator;
    [SerializeField] PlayerHitBox hitBox;

    public static bool attacking = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            attackInd = ValidateAndFindAttack();

            if (attackInd != -1)
                Attack();
        }

        ExitAttack();
    }

    int ValidateAndFindAttack()
    {
        //cycle all combos
        for (int i = 0; i < attacks.Count; i++)
        {
            //Check for matching requirements
            bool validation = CheckActivationReqs(attacks[i].activationReq);

            if (validation)
                return i;
        }

        return -1;
    }

    bool CheckActivationReq(ActivationReq req)
    {
        switch (req)
        {
            case ActivationReq.Grounded:
                if (PlayerController.below)
                    return true;
                break;

            case ActivationReq.AirBorn:
                if (!PlayerController.below)
                    return true;
                break;

            case ActivationReq.Moving:
                if (PlayerController.horizontal != 0)
                    return true;
                break;
        }

        return false;
    }

    bool CheckActivationReqs(List<ActivationReq> req)
    {
        bool eval = true;

        for (int i = 0; i < req.Count; i++)
        {
            if (CheckActivationReq(req[i]) == false)
            {
                eval = false;
                break;
            }
        }

        return eval;
    }

    bool CheckCancelCon(CancelationCon cancelCon)
    {
        switch (cancelCon)
        {
            case CancelationCon.Grounded:
                if (PlayerController.below)
                    return true;
                break;

            case CancelationCon.HitWall:
                if (AnimatorController.lookingRight)
                {
                    if (PlayerController.right)
                        return true;
                }
                else
                {
                    if (PlayerController.left)
                        return true;
                }
                break;
        }

        return false;
    }

    bool CheckCancelCons(List<CancelationCon> cancelCons)
    {
        bool eval = true;

        for (int i = 0; i < cancelCons.Count; i++)
        {
            if (CheckCancelCon(cancelCons[i]) == false)
            {
                eval = false;
                break;
            }
        }

        return eval;
    }

    void Attack()
    {
        if (Time.time - lastAttackedTime > .1f)
        {
            CancelInvoke(nameof(EndCombo));

            //Variables
            AttackSO currentAttack = attacks[attackInd].attack;

            attacking = true;
            animator.runtimeAnimatorController = currentAttack.animatorOV;
            hitBox.damage = currentAttack.damage;

            //Effects
            foreach (Effects effect in currentAttack.effect)
            {
                switch (effect)
                {
                    case Effects.VelocityOverride:
                        PlayerController.Instance.OverrideVelocity(currentAttack.OverrideVel,
                                                                    AnimatorController.lookingRight);
                        break;
                    case Effects.CantMove:
                        PlayerController.moveOverride = true;
                        break;
                }
            }

            //Animation
            AnimatorController.PlayAnimation("Attack");
        }
    }

    void ExitAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .99f && attacks[attackInd].CancelOnAnimEnd)
            Invoke(nameof(EndCombo), 0);

        if (attacks[attackInd].cancelationCon.Count == 0)
            return;
        else if (CheckCancelCons(attacks[attackInd].cancelationCon))
            Invoke(nameof(EndCombo), 0);
    }

    void EndCombo()
    {
        lastAttackedTime = Time.time;
        attacking = false;
        PlayerController.moveOverride = false;
    }
}

[System.Serializable]
public class Attack
{
    public string name;
    public List<ActivationReq> activationReq;
    public AttackSO attack;

    public List<CancelationCon> cancelationCon;
    public bool CancelOnAnimEnd = true;
}

public enum ActivationReq
{
    Grounded,
    AirBorn,
    Moving
}

public enum CancelationCon
{
    Grounded,
    HitWall
}
