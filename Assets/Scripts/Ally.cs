using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    protected bool skillCalled = false;

    public void CallSkill ()
    {
        skillCalled = true;
    }
}
