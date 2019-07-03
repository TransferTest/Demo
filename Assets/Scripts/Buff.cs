using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    protected string buffId;
    protected Unit target;
    protected Unit caster;
    protected float remainTime;

    public Buff()
    {
        this.buffId = "null";
        this.target = null;
        this.caster = null;
        this.remainTime = 0;
    }
    public Buff(string buffId, float time, Unit target, Unit caster)
    {
        this.buffId = buffId;
        remainTime = time;
        this.target = target;
        this.caster = caster;
    }
    protected void Init(string buffId, float time, Unit target, Unit caster)
    {
        this.buffId = buffId;
        remainTime = time;
        this.target = target;
        this.caster = caster;
    }
    public void SetBuffId(string buffId)
    {
        this.buffId = buffId;
    }
    public void SetTarget(Unit target)
    {
        this.target = target;
    }
    public void SetCaster(Unit caster)
    {
        this.caster = caster;
    }
    public void SetRemainTime(float remainTime)
    {
        this.remainTime = remainTime;
    }
    public string GetBuffId ()
    {
        return buffId;
    }
    public Unit GetTarget()
    {
        return target;
    }
    public Unit GetCaster()
    {
        return caster;
    }
    public float GetRemainTime()
    {
        return remainTime;
    }
    public void updateTime()
    {
        SetRemainTime(remainTime - Time.deltaTime);
    }
}

// Buff: Increase outcoming damage
public class IncOutDamage : Buff
{
    float coeff;
    public IncOutDamage(float coeff, float time, Unit target, Unit caster)
    {
        Init("IncOutDamage", time, target, caster);
        this.coeff = coeff;
    }
    public float GetCoeff()
    {
        return coeff;
    }
}

// Debuff: Increase incoming damage
public class IncIncDamage : Buff
{
    float coeff;
    public IncIncDamage(float coeff, float time, Unit target, Unit caster)
    {
        Init("IncIncDamage", time, target, caster);
        this.coeff = coeff;
    }
    public float GetCoeff()
    {
        return coeff;
    }
}

// Buff: Shield
// Act as additional HP
public class Shield : Buff
{
    int amount;
    public Shield(int shield, float time, Unit target, Unit caster)
    {
        Init("Shield", time, target, caster);
        this.amount = shield;
    }
    public int GetAmount()
    {
        return amount;
    }
    public void DecrementShield (int damage)
    {
        amount -= damage;
    }
}