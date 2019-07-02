using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : Ally
{
    public int shieldAmount;
    public float shieldTime;
    protected override IEnumerator AutoAttack()
    {
        attackRemainTime = delay;
        while (true)
        {
            if (attackRemainTime > 0)
                attackRemainTime -= Time.deltaTime;
            if (skillCoolDown > 0)
                skillCoolDown -= Time.deltaTime;

            if (attackRemainTime <= 0)
            {
                AutoTarget();
                if (Target != null)
                {
                    Attack(Target);
                    attackRemainTime = delay;
                }
            }
            if (moveCalled == true)
            {
                state = State.Move;
                yield break;
            }
            if (skillCalled && skillCoolDown <= 0)
            {
                skillCoolDown = skillCoolTime;
                state = State.Skill;
            }
            yield return null;
            if (state != State.AutoAttack)
                yield break;
        }
    }

    private IEnumerator Skill()
    {
        while (true)
        {
            Ally tankTarget = this;
            if (SceneManager.Instance.allies.Count > 0)
            {
                tankTarget = SceneManager.Instance.allies[0];
            }
            if (designatedSkillTarget != null && designatedSkillTarget is Ally)
            {
                tankTarget = (Ally)designatedSkillTarget;
            }
                
            Shield shld = new Shield(shieldAmount, shieldTime, tankTarget, this);
            tankTarget.GetBuff(shld);
            skillCalled = false;
            state = State.AutoAttack;
            yield break;
        }
    }

    protected override void AutoTarget()
    {
        if (SceneManager.Instance.enemies.Count > 0)
            SetAutoTarget(SceneManager.Instance.enemies[0]);
    }
}
