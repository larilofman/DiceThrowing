using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiceRollSetup
{
    public List<string> diceNames;
    public List<int> diceAmounts;
    public List<int> diceBonusPenalties;
    public int bonus;

    //public DiceRollSetup(List<string> diceNames, List<int> diceAmounts, List<int> diceBonusPenalties, int bonus)
    //{
    //    this.diceNames = diceNames;
    //    this.diceAmounts = diceAmounts;
    //    this.diceBonusPenalties = diceBonusPenalties;
    //    this.bonus = bonus;
    //}
}
