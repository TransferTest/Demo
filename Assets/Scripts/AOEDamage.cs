using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : AOE
{
    int damage;
    public void Initialize(float timeRemain, int radius, int center, int damage)
    {
        Init("AOEDamage", timeRemain, radius, center);
        this.damage = damage;
    }
    protected override void Activate()
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