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
    protected override void AutoTarget ()
    {
        if (SceneManager.Instance.allies.Count > 0)
            SetAutoTarget(SceneManager.Instance.allies[0]);
    }
}
