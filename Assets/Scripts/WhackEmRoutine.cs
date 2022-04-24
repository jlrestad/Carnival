using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackEmRoutine
{
    public int up;
    public int taunt;
    public bool addTaunt;

    public WhackEmRoutine()
    {
        //choose random enemy to put up
        up = UnityEngine.Random.Range(0, 5);

        // choose random enemy to put in taunt
        taunt = UnityEngine.Random.Range(0, 5);

        // choose if taunting or not
        int which = UnityEngine.Random.Range(0, 100);
        if (which < 50)
        {
            addTaunt = true;
        }
        else
        {
            addTaunt = false;
        }
    }

    public override string ToString()
    {
        return " up " + up + " taunt " + taunt + " addTaunt " + addTaunt;
    }
}
