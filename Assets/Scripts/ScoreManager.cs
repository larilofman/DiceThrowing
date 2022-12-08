using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public EventManager eventManager;
    public RectTransform scoreTextParent;
    public GameObject scoreTextPrefab;
    public TextMeshProUGUI totalScoreText;
    private List<DiceResult> currentScores = new List<DiceResult>();
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
            currentScores.Add(new DiceResult(diceResult.result, diceResult.type));
        }

        foreach (BonusAdjust bonus in bonuses)
        {
            string name = bonus.displayName;
            int amount = bonus.GetAmount();
            currentScores.Add(new DiceResult(amount, name, false));
        }
    }

    static int SortByType(DiceResult d1, DiceResult d2)
    {
        return d1.typeN.CompareTo(d2.typeN);
    }

    public void InsertScore()
    {
        foreach (DiceResult diceResult in currentScores)
        {
            totalScore += diceResult.result;

            GameObject scoreTextObject = Instantiate(scoreTextPrefab, scoreTextParent);

            TextMeshProUGUI scoreText = scoreTextObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            scoreText.text = diceResult.result.ToString();

            TextMeshProUGUI typeText = scoreTextObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            typeText.text = $"({diceResult.type})";
        }
        totalScoreText.text = $"Total: {totalScore}";

    }
}
