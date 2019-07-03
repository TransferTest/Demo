using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public enum EnemyType { fishman, skeleton }
    EnemyType enemyType;
    public EnemyType Type{ get { return enemyType; } }

    public void Init(EnemyType type)
    {
        enemyType = type;
        DefaultSprite = Resources.Load<Sprite>("Sprites/Units/" + Type.ToString());
        AttackingSprite = Resources.Load<Sprite>("Sprites/Units/" + Type.ToString()+"_attacking");

        SetRandomAttackRemainTime();
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
            if (skillCoolDown <= 0)
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
            int pos = Random.Range(0, 5);
            SceneManager.Instance.SpawnAOEDamage(2, 0, pos, atk);
            state = State.AutoAttack;
            yield break;
        }
    }

    protected override void AutoTarget ()
    {
        if (SceneManager.Instance.allies.Count > 0)
            SetAutoTarget(SceneManager.Instance.allies[0]);
    }
}
