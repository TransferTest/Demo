using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ally : Unit
{
    public GameObject MyUI;
    private Text infoText;
    protected bool skillCalled = false;

    public new void Start()
    {
        base.Start();
        infoText = MyUI.transform.Find("InfoText").GetComponent<Text>();
        infoText.text = "변경";
    }
    public new void Update()
    {
        base.Update();
        infoText.text = "체력 " + HP + " / " + maxHP +
            "\n공격력 " + atk +
            "\n보호막 " + GetTotalShield() +
            "\n딜레이 " + delay +
            "\n공격남은시간" + Util.Truncate(attackRemainTime) +
            "\n스킬 쿨타임 " + skillCoolTime +
            "\n스킬남은시간" + Util.Truncate(skillCoolDown) +
            "\nstate " + state.ToString();
    }
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
                moveCalled = false;
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

    public void CallSkill ()
    {
        skillCalled = true;
    }
}
