using System;
using System.Collections;
using System.Collections.Generic;

public static class Util
{
    public static double Truncate(float num)
    {
        return Math.Round(num * 100) / 100;
    }
}
