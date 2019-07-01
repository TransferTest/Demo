using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject HP_bar;
    public int max_HP;
    public int atk;
    public float delay;
    public float skillCoolTime;

    protected Unit target;
    protected enum State { AutoAttack, Skill, Move }
    protected State state = State.Move;

    protected float attackTime;
    protected int HP;
    protected float skillCoolDown;

    Vector3 dest = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        attackTime = 0;
        HP = max_HP;
        //SetTarget(this);
        StartCoroutine(FSM());
    }

    // Update is called once per frame
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
        if (HP > max_HP)
        {
            HP = max_HP;
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
        target = t;
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
        HP_bar.transform.localScale = new Vector3(((float)HP / (float)max_HP), 1, 1);
    }
}
