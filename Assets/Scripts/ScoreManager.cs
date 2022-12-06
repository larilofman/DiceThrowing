using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public UIManager uiManager;
    public CamMover camMover;
    private DiceThrower diceThrower;
    private EventManager eventManager;
    private List<DiceScore> diceScores = new List<DiceScore>();
    private Bonus bonus;

    void Start()
    {
        diceThrower = GetComponent<DiceThrower>();
        eventManager = GetComponent<EventManager>();

    }

    public void ResetScore()
    {
        diceScores.Clear();
    }

    void CalculateScore()
    {
        List<DiceResult> diceResults = new List<DiceResult>();
        foreach (DiceScore diceScore in diceScores)
        {
            string diceName = diceScore.gameObject.name.Split("(")[0];
            int score = diceScore.GetResult();
            diceResults.Add(new DiceResult(score, diceName));
            
        }

        diceResults.Sort(SortByType);

        //string scoreString = "";
        //foreach (DiceResult diceResult in diceResults)
        //{
        //    string result = diceResult.result.ToString();
        //    string paddedResult = result.PadRight(5 - result.Length);
        //    scoreString += $"<b>{paddedResult}</b> ({diceResult.type})\n";
        //}
        uiManager.InsertScore(diceResults, bonus);
    }

    static int SortByType(DiceResult d1, DiceResult d2)
    {
        return d1.typeN.CompareTo(d2.typeN);
    }

    public void SetBonus(Bonus _bonus)
    {
        bonus = _bonus;
    }
}
