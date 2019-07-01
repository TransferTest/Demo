using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected override void AutoTarget ()
    {
        if (SceneManager.Instance.allies.Count > 0)
            SetTarget(SceneManager.Instance.allies[0]);
    }
}
