using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Ally
{

    private IEnumerator Skill ()
    {
        while (true)
        {
            Ally healTarget = this;
            if (SceneManager.Instance.allies.Count > 0)
                healTarget = SceneManager.Instance.allies[0];
            if (designatedSkillTarget != null && designatedSkillTarget is Ally)
                healTarget = (Ally)designatedSkillTarget;
            Heal(healTarget);
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
