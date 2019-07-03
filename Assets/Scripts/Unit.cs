using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public HPBar hpbar;
    public GameObject UnitImage;

    public Sprite DefaultSprite;
    public Sprite AttackingSprite;

    public int order;

    public int maxHP;
    protected int HP;
    public int atk;
    public float delay;
    public float skillCoolTime;

    protected Unit autoTarget;
	protected Unit designatedTarget;
    protected Unit designatedSkillTarget;
    protected enum State { AutoAttack, Skill, Move }
	protected State state = State.AutoAttack;

    protected List<Buff> buffs;

    protected float attackRemainTime = 0;
    protected float skillCoolDown;
    protected bool moveCalled = false;

    Vector3 dest = new Vector3(0,0,0);

    public void Start()
    {
        HP = maxHP;
        hpbar = transform.GetComponentInChildren<HPBar>();
        hpbar.Init(maxHP, HP);
        buffs = new List<Buff>();
        SetAutoTarget(this);
        StartCoroutine(FSM());
    }

    public void Update()
    {
        if(state == State.AutoAttack && Target != null && attackRemainTime < 0.5 * delay)
        {
            UnitImage.GetComponent<SpriteRenderer>().sprite = AttackingSprite;
        }
        else
        {
            UnitImage.GetComponent<SpriteRenderer>().sprite = DefaultSprite;
        }
    }

    protected IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    public void SetRandomAttackRemainTime()
    {
        attackRemainTime = delay * UnityEngine.Random.Range(0.0f, 0.99f);
    }

    public void UpdateBuffRemainTime ()
    {
        int i = 0;
        while (i < buffs.Count)
        {
            Buff b = buffs[i];
            b.updateTime();
            if (b.GetRemainTime() < 0)
            {
                buffs.Remove(b);
                continue;
            }
            i += 1;
        }
    }

    public void GetDamage(int damage)
    {
        //Decrement shield first
        int damageTaken = damage;
        if (damage > 0)
        {
            foreach(Buff b in buffs)
            {
                if (b is Shield)
                {
                    if (((Shield)b).GetShield() > damageTaken)
                    {
                        ((Shield)b).DecrementShield(damageTaken);
                        damageTaken = 0;
                        hpbar.UpdateShield(GetTotalShield());
                        break;
                    }
                    else
                    {
                        damageTaken -= ((Shield)b).GetShield();
                        buffs.Remove(b);
                        hpbar.UpdateShield(GetTotalShield());
                        continue;
                    }
                }
            }
        }
        //Decrement HP
        HP -= damageTaken;
        if (HP < 0)
        {
            HP = 0;
            Die();
        }
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        //Update HP bar
        hpbar.UpdateHP(HP);
    }

    public void GetBuff (Buff b)
    {
        b.SetTarget(this);
        buffs.Add(b);
    }

    protected int GetTotalShield()
    {
        int total = 0;
        foreach(Buff buff in buffs)
        {
            if(buff is Shield)
            {
                total += ((Shield)buff).GetShield();
            }
        }
        return total;
    }

    void Die()
    {
        SceneManager.Instance.UnitDied(this);
        Destroy(gameObject);
    }
    public void SetAutoTarget (Unit unit)
    {
        autoTarget = unit;
    }
    public void SetDesignatedTarget (Unit unit)
    {
        designatedTarget = unit;
    }
    public void SetDesignatedSkillTarget(Unit unit)
    {
        designatedSkillTarget = unit;
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
                moveCalled = false;
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
        target.GetDamage (CalculateAttackDamage(atk));
    }

    protected void Heal(Unit target)
    {
        target.GetDamage(-atk);
    }

    protected virtual void AutoTarget ()
    {
        //Debug.Log("Should override AutoTarget ()");
    }
    protected Unit Target
    {
        get
        {
            if (designatedTarget != null)
            {
                return designatedTarget;
            }
            else
            {
                return autoTarget;
            }
        }
    }

    private int CalculateAttackDamage (int atk)
    {
        float coeff = 1.0f;
        for (int i = 0; i < buffs.Count; i++)
        {
            Buff b = buffs[i];
            if (b is IncOutDamage)
            {
                coeff = coeff * ((IncOutDamage)b).GetCoeff();
            }
        }
        return (int)(coeff * (float)atk);
    }
}
