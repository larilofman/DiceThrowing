using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private int _bonus;
    public void Init(int bonus)
    {
        _bonus = bonus;
    }

    public int GetBonus()
    {
        return _bonus;
    }
}
