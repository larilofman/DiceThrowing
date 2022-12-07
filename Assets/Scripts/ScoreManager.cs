using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public EventManager eventManager;
    public RectTransform scoreTextParent;
    public TextMeshProUGUI scoreTextPrefab;
    public TextMeshProUGUI totalScoreText;
    private Dictionary<string, int> currentScores = new Dictionary<string, int>();
    private int totalScore = 0;

    void Start()
    {
        eventManager.EventAllDiceStopped.AddListener(AllDiceStoppedEventHandler);
    }

    void AllDiceStoppedEventHandler(List<DiceScore> diceScores, List<BonusAdjust> bonuses)
    {
        CalculateScore(diceScores, bonuses);
        InsertScore();
    }

    void CalculateScore(List<DiceScore> diceScores, List<BonusAdjust> bonuses)
    {
        List<DiceResult> results = new List<DiceResult>();
        foreach (DiceScore diceScore in diceScores)
        {
            string diceName = diceScore.gameObject.name.Split("(")[0];
            int score = diceScore.GetResult();
            results.Add(new DiceResult(score, diceName));

        }

        results.Sort(SortByType);

        foreach (DiceResult diceResult in results)
        {
            currentScores.Add(diceResult.type, diceResult.result);
        }

        foreach (BonusAdjust bonus in bonuses)
        {
            string name = bonus.displayName;
            int amount = bonus.GetAmount();
            currentScores.Add(name, amount);
        }
    }

    static int SortByType(DiceResult d1, DiceResult d2)
    {
        return d1.typeN.CompareTo(d2.typeN);
    }

    public void InsertScore()
    {
        //Dictionary<string, int> diceAmounts = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> entry in currentScores)
        {
            totalScore += entry.Value;
            string result = entry.Value.ToString();
            string paddedResult = result.PadRight(5 - result.Length);
            TextMeshProUGUI scoreText = Instantiate(scoreTextPrefab, scoreTextParent);
            scoreText.text = $"<b>{paddedResult}</b> ({entry.Key})";

            //if (diceAmounts.ContainsKey(diceResult.type))
            //{
            //    diceAmounts[diceResult.type] = diceAmounts[diceResult.type] + 1;
            //}
            //else
            //{
            //    diceAmounts.Add(diceResult.type, 1);
            //}

        }
        //int bonusAmount = bonus.GetResult();
        //if (bonusAmount != 0)
        //{
        //    totalScore += bonusAmount;
        //    string bonusString = bonusAmount.ToString();
        //    string paddedBonus = bonusString.PadRight(5 - bonusString.Length);
        //    TextMeshProUGUI bonusText = Instantiate(scoreTextPrefab, scoreTextParent);
        //    bonusText.text = $"<b>{paddedBonus}</b> (Bonus)";
        //}

        //UpdateTitle(diceAmounts, bonusAmount);

        totalScoreText.text = $"Total: <b>{totalScore}</b>";

    }
}
