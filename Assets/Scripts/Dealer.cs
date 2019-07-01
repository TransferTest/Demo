using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : Ally
{
    private bool skillCall = false;

    private IEnumerator AutoAttack ()
    {
        while (true)
        {
            if (attackTime > 0)
                attackTime -= Time.deltaTime;
            if (skillCoolDown > 0)
                skillCoolDown -= Time.deltaTime;

            if (attackTime <= 0)
            {
                AutoTarget();
                if (autoTarget != null)
                {
                    Attack(autoTarget);
                    attackTime = delay;
                }
            }
            if (skillCall && skillCoolDown <= 0)
            {
                skillCoolDown = skillCoolTime;
                state = State.Skill;
            }
            yield return null;
            if (state != State.AutoAttack)
                yield break;
        }
    }

    private IEnumerator Skill ()
    {
        while (true)
        {
            if (skillCall && autoTarget != null)
            {
                Heal(autoTarget);
                attackTime = delay;
                skillCall = false;
                state = State.AutoAttack;
            }
            yield return null;
            if (state != State.Skill)
                yield break;
        }
    }

    public void CallSkill ()
    {
        skillCall = true;
    }

    private void AutoTarget ()
    {
        if (SceneManager.Instance.enemies.Count > 0)
            SetTarget(SceneManager.Instance.enemies[0]);
    }
}
