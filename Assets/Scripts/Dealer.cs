﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : Ally
{
    protected override IEnumerator AutoAttack ()
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
                if (autoTarget != null)
                {
                    Attack(autoTarget);
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

    protected override void AutoTarget ()
    {
        if (SceneManager.Instance.enemies.Count > 0)
            SetTarget(SceneManager.Instance.enemies[0]);
    }
}
