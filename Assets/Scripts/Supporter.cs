using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supporter : Ally
{
    public float buffCoeff;
    public float buffTime;

    private IEnumerator Skill()
    {
        while (true)
        {
            Ally suppTarget = this;
            if (SceneManager.Instance.allies.Count > 0)
            {
                suppTarget = SceneManager.Instance.allies[0];
            }
            if (designatedSkillTarget != null && designatedSkillTarget is Ally)
            {
                suppTarget = (Ally)designatedSkillTarget;
            }

            IncOutDamage b = new IncOutDamage(buffCoeff, buffTime, suppTarget, this);
            suppTarget.GetBuff(b);
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
