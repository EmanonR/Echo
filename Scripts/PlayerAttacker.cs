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

    [SerializeField] PlayerHitBox hitBox;

    public static bool attacking = false;

    public static event Action<Vector2, AnimationClip, Attack> onAttack;

    PlayerCollisionHandler colhandler;

    private void Awake()
    {
        colhandler = GetComponent<PlayerCollisionHandler>();
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
                if (colhandler.below)
                    return true;
                break;

            case ActivationReq.AirBorn:
                if (!colhandler.below)
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
                if (colhandler.below)
                    return true;
                break;

            case CancelationCon.HitWall:
                if (AnimatorController.lookingRight)
                {
                    if (colhandler.right)
                        return true;
                }
                else
                {
                    if (colhandler.left)
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
            attacking = true;

            AttackSO currentAttack = attacks[attackInd].attack;

            //HitBox
            Vector2 hbOffset = new(AnimatorController.lookingRight ? currentAttack.offset.x : -currentAttack.offset.x, currentAttack.offset.y);
            hitBox.hitBox.offset = hbOffset;
            hitBox.hitBox.size = currentAttack.size;
            hitBox.damage = currentAttack.damage;
            hitBox.EnableHitBox();

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
            AnimatorController.animator.runtimeAnimatorController = currentAttack.animatorOV;
            AnimatorController.PlayAnimation("Attack");

            //onAttack?.Invoke(transform.position, AnimatorController.animator.GetCurrentAnimatorClipInfo(0)[0].clip, currentAttack);
            onAttack?.Invoke(transform.position, currentAttack.animatorOV["Attack"], attacks[attackInd]);
        }
    }

    void ExitAttack()
    {
        if (!AnimatorController.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

        if (AnimatorController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .99f && attacks[attackInd].CancelOnAnimEnd)
            Invoke(nameof(EndCombo), 0);

        if (attacks[attackInd].cancelationCon.Count == 0)
            return;
        else if (CheckCancelCons(attacks[attackInd].cancelationCon))
            Invoke(nameof(EndCombo), 0);
    }

    void EndCombo()
    {
        hitBox.DissableHitBox();
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
