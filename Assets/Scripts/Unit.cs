using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject HPBar;
    public GameObject UnitImage;
    public int order;

    public int maxHP;
    protected int HP;
    public int atk;
    public float delay;
    public float skillCoolTime;

    protected Unit autoTarget;
	protected Unit designatedTarget;
    protected enum State { AutoAttack, Skill, Move }
	protected State state = State.AutoAttack;

    protected float attackRemainTime;
    protected float skillCoolDown;
    protected bool moveCalled = false;

    Vector3 dest = new Vector3(0,0,0);

    void Start()
    {
        attackRemainTime = 0;
        HP = maxHP;
        SetAutoTarget(this);
        StartCoroutine(FSM());
    }

    void Update()
    {
    }

    protected IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            HP = 0;
            Die();
        }
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        UpdateHP();
    }
    void Die()
    {
        if (this is Enemy)
            SceneManager.Instance.EnemyDied((Enemy)this);
        Destroy(gameObject);
    }
    public void SetAutoTarget (Unit t)
    {
        autoTarget = t;
    }

    protected IEnumerator Move()
    {
        float remainTime = 1f;
        Vector3 start = this.gameObject.transform.position;
        while (true)
        {
            remainTime -= Time.deltaTime;
            if(remainTime <= 0)
            {
                gameObject.transform.position = dest;
                state = State.AutoAttack;
                yield break;
            }
            gameObject.transform.position = start * remainTime + dest * (1 - remainTime);
            yield return null;
        }
    }

    protected virtual IEnumerator AutoAttack()
    {
        attackRemainTime = delay;
        while (true)
        {
            if (attackRemainTime > 0)
                attackRemainTime -= Time.deltaTime;
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
            yield return null;
        }
    }

    public void MoveToPosition(Vector3 dest)
    {
        moveCalled = true;
        this.dest = dest;
    }

    protected void Attack (Unit target)
    {
        target.GetDamage (atk);
    }

    protected void Heal(Unit target)
    {
        target.GetDamage(-atk);
    }

    protected virtual void AutoTarget ()
    {
    }

    private void UpdateHP()
    {
        HPBar.transform.localScale = new Vector3(((float)HP / (float)maxHP), 1, 1);
    }
}
