using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE
{
    protected float timeRemain;
    protected int radius;
    protected int center;

    public AOE ()
    {
        timeRemain = 10;
        radius = 0;
    }

    public AOE (float timeRemain, int radius, int center)
    {
        this.timeRemain = timeRemain;
        this.radius = radius;
        this.center = center;
    }

    protected void Init(float timeRemain, int radius, int center)
    {
        this.timeRemain = timeRemain;
        this.radius = radius;
        this.center = center;
    }

    // Update timeRemain and returns true if AOE activated
    // Just delete this object if CheckTime() returns true
    //
    // if ( aoes[i].CheckTime() )
    // {
    //    aoes.Remove(aoes[i]);
    // {
    public bool CheckTime ()
    {
        timeRemain -= Time.deltaTime;
        if (timeRemain <= 0)
        {
            Activate();
            return true;
        }
        return false;
    }

    protected virtual void Activate ()
    {
        // Call it when timeRemain goes under 0
    }
}

public class AOEDamage : AOE
{
    int damage;
    public AOEDamage (float timeRemain, int radius, int center, int damage)
    {
        Init(timeRemain, radius, center);
        this.damage = damage;
    }
    protected override void Activate ()
    {
        List<Ally> targets = new List<Ally>();
        for (int i = center - radius; i <= center + radius; i++)
        {
            Ally t = SceneManager.Instance.AllyAtOrder(i);
            if (t != null)
            {
                targets.Add(t);
            }
        }
        foreach (Ally t in targets)
        {
            t.GetDamage(damage);
        }
    }
}