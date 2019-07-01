using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject HPBar;
    public int maxHP;
    public int atk;
    public float delay;
    public float skillCoolTime;

    protected Unit autoTarget;
	protected Unit designatedTarget;
    protected enum State { AutoAttack, Skill, Move }
	protected State state = State.AutoAttack;

    protected float attackTime;
    protected int HP;
    protected float skillCoolDown;

    Vector3 dest = new Vector3(0,0,0);

    void Start()
    {
        attackTime = 0;
        HP = maxHP;
        SetTarget(this);
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
            SceneManager.Instance.enemies.Remove((Enemy)this);
        Destroy(gameObject);
    }
    public void SetTarget (Unit t)
    {
        autoTarget = t;
    }

    protected IEnumerator Move()
    {
        float remainTime = 1.0f;
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

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            yield return null;
        }
    }

    protected void Attack (Unit target)
    {
        target.GetDamage (atk);
    }

    protected void Heal(Unit target)
    {
        target.GetDamage(-atk);
    }

    private void UpdateHP()
    {
        HPBar.transform.localScale = new Vector3(((float)HP / (float)maxHP), 1, 1);
    }
}
