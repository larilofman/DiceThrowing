using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Score
{
    private int _bonus;
    public void Init(int bonus)
    {
        _bonus = bonus;
    }

    public override int GetResult()
    {
        return _bonus;
    }
}
