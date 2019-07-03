using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : Ally
{
    public int shieldAmount;
    public float shieldTime;

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
            tankTarget.hpbar.UpdateShield(tankTarget.GetTotalShield());
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
