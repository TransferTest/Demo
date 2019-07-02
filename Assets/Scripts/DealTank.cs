using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealTank : Ally
{
    public int skillDamage;

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

    protected override void AutoTarget()
    {
        if (SceneManager.Instance.enemies.Count > 0)
            SetAutoTarget(SceneManager.Instance.enemies[0]);
    }
}
