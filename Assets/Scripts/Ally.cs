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

    protected Unit Target
    {
        get
        {
            if(designatedTarget != null)
                return designatedTarget;
            else
                return autoTarget;
        }
    }
}
