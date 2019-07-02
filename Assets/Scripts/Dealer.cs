using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : Ally
{
    public int skillDamage;
    protected override IEnumerator AutoAttack ()
    {
        attackRemainTime = 0.3f * delay;
        while (true)
        {
            if (attackRemainTime > 0)
                attackRemainTime -= Time.deltaTime;
            if (skillCoolDown > 0)
                skillCoolDown -= Time.deltaTime;

            if (attackRemainTime < 0.3f)
            {
                UnitImage.GetComponent<SpriteRenderer>().sprite = AttackingSprite;
            }

            if (attackRemainTime <= 0)
            {
                AutoTarget();
                if (Target != null)
                {
                    Attack(Target);
                    attackRemainTime = delay;
                    UnitImage.GetComponent<SpriteRenderer>().sprite = DefaultSprite;
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
            Enemy dealerTarget = null;
            if (this.Target is Enemy)
                dealerTarget = (Enemy)this.Target;
            if ((designatedSkillTarget != null) && (designatedSkillTarget is Enemy))
                dealerTarget = (Enemy)designatedSkillTarget;
            if (dealerTarget == null)
            {
                AutoTarget();
                yield return null;
                continue;
            }

            int atk_temp = atk;
            atk = skillDamage;
            Attack(dealerTarget);
            atk = atk_temp;
            skillCalled = false;
            state = State.AutoAttack;
            yield break;
        }
    }

    protected override void AutoTarget ()
    {
        if (SceneManager.Instance.enemies.Count > 0)
            SetAutoTarget(SceneManager.Instance.enemies[0]);
    }
}
