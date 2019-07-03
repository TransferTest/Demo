using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    protected Sprite AOEDefault;
    protected Sprite AOEActivated;

    protected float timeRemain;
    protected int radius;
    protected int center;
    protected string id;

    public AOE ()
    {
        timeRemain = 10;
        radius = 0;
    }

    public AOE (string id, float timeRemain, int radius, int center)
    {
        this.timeRemain = timeRemain;
        this.radius = radius;
        this.center = center;
        this.id = id;
    }

    protected void Init(string id, float timeRemain, int radius, int center)
    {
        this.timeRemain = timeRemain;
        this.radius = radius;
        this.center = center;
        this.id = id;

        AOEDefault = Resources.Load<Sprite>("Sprites/Effects/" + id);
        AOEActivated = Resources.Load<Sprite>("Sprites/Effects/" + id + "Activated");

        gameObject.GetComponent<SpriteRenderer>().sprite = AOEDefault;
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
            gameObject.GetComponent<SpriteRenderer>().sprite = AOEActivated;
            return true;
        }
        return false;
    }

    protected virtual void Activate ()
    {
        // Call it when timeRemain goes under 0
    }
}