using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiceRollSetup
{
    public List<DiceRollData> diceRollData;
    public int bonus;
    public string name;

    //public DiceRollSetup(List<string> diceNames, List<int> diceAmounts, List<int> diceBonusPenalties, int bonus)
    //{
    //    this.diceNames = diceNames;
    //    this.diceAmounts = diceAmounts;
    //    this.diceBonusPenalties = diceBonusPenalties;
    //    this.bonus = bonus;
    //}
}

[System.Serializable]
public class DiceRollData
{
    public string name;
    public int amount;
    public bool bonus;
    public bool penalty;
}
